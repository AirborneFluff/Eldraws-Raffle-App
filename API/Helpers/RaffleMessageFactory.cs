using Discord;
using Microsoft.EntityFrameworkCore;
using RaffleApi.Configurations;
using RaffleApi.Data;
using RaffleApi.Entities;
using RaffleApi.Extensions;
using RaffleApi.Utilities;
using EmbedBuilderExtensions = RaffleApi.Extensions.EmbedBuilderExtensions;

namespace RaffleApi.Helpers;

public class RaffleMessageFactory
{
    private readonly DataContext _context;
    private readonly int _raffleId;
    private readonly RaffleMessageFactoryConfig _config;
    private LookAheadEnumerator<string> _entryEnumerator;
    private List<RaffleEntry> _entries = new();
    private List<RafflePrize> _prizes = new();
    private Raffle _raffle = null!;
    
    private readonly int _maxEmbedLength = 6000;
    private readonly int _maxFieldCount = 25;

    private readonly string _noPrizesMessage = "No prizes have been listed yet";
    private readonly string _noEntriesMessage = "No entries have been made yet";
    private readonly string _pendingEmoji = "<a:threepointsanima:1005525060490117191>";
    private readonly string _diceEmoji = "<a:dices:1172979983321411676>";

    private int _entryPages;

    private IList<EmbedBuilder> _messages = new List<EmbedBuilder>();
    public EmbedBuilder PrimaryMessage => _messages.First();
    public EmbedBuilder[] AdditionalMessages => _messages.Skip(1).ToArray();
    
    public RaffleMessageFactory(DataContext context, int raffleId, RaffleMessageFactoryConfig config)
    {
        _config = config;
        _context = context;
        _raffleId = raffleId;
    }

    public async Task BuildPrimaryMessage()
    {
        await LoadRaffle();
        
        var embed = await CreatePrimaryEmbed();
        if (_messages.FirstOrDefault() is null)
        {
            _messages.Add(embed);
            return;
        }

        _messages.RemoveAt(0);
        _messages.Insert(0, embed);
    }
    
    public async Task BuildMessages()
    {
        await LoadRaffle();
        
        _messages.Clear();
        
        var primaryEmbed = await CreatePrimaryEmbed();
        _messages.Add(primaryEmbed);

        while (_entryEnumerator.HasNext)
        {
            var embed = CreateAdditionalEmbed();
            _messages.Add(embed);
        }
    }

    private async Task LoadRaffle()
    {
        _raffle = await _context.Raffles.SingleAsync(raffle => raffle.Id == _raffleId);

        _entries = await _context.Entries
            .Where(entry => entry.RaffleId == _raffleId)
            .Where(entry => entry.LowTicket != 0)
            .Include(entry => entry.Entrant)
            .OrderBy(entry => entry.LowTicket)
            .ToListAsync();

        _prizes = await _context.Prizes
            .Where(prize => prize.RaffleId == _raffleId)
            .Include(prize => prize.Winner)
            .OrderBy(prize => prize.Place)
            .ToListAsync();

        _entryEnumerator = new LookAheadEnumerator<string>(_entries.Select(entry => entry.ToString()).GetEnumerator());
    }

    private EmbedBuilder CreateAdditionalEmbed()
    {
        var embed = new EmbedBuilder()
        {
            Title = _raffle.Title
        };
        
        AddEntryFields(embed);
        return embed;
    }

    private async Task<EmbedBuilder> CreatePrimaryEmbed()
    {
        var embed = new EmbedBuilder()
        {
            Title = _raffle.Title,
            Description = _raffle.Description ?? _raffle.DefaultDescription()
        };
        AddDateFooter(embed);
        
        if (_config.RollValue is not null) AddRollingField(embed, (int)_config.RollValue);
        if (_config.ShowWinners) AddWinnersFields(embed);
        AddPrizesFields(embed);
        AddEntryFields(embed);
        
        return embed;
    }

    private void AddDateFooter(EmbedBuilder embed)
    {
        var currentTime = DateTime.UtcNow.ToString("dd-MMM @ hh:mm tt");
        embed.Footer = new EmbedFooterBuilder()
            .WithText($"Last updated: {currentTime} UTC");
    }

    private void AddWinnersFields(EmbedBuilder embed)
    {
        if (!_prizes.Any())
        {
            embed.AddField("Winners", _noPrizesMessage);
            return;
        }

        var fields = EmbedUtilities.CreateLinedFields("Winners", _prizes.Select(GetWinnerLine));
        embed.Fields.AddRange(fields);
    }

    private void AddRollingField(EmbedBuilder embed, int value)
    {
        var content = "Rolling... " + (_config.UseCustomEmojis ? _diceEmoji : String.Empty) + value;
        embed.AddField("Roll", content);
    }

    private void AddPrizesFields(EmbedBuilder embed)
    {
        if (!_prizes.Any())
        {
            embed.AddField("Prizes", _noPrizesMessage);
            return;
        }

        var fields = EmbedUtilities.CreateLinedFields("Prizes", _prizes.Select(prize => prize.ToString()));
        embed.Fields.AddRange(fields);
    }

    private void AddEntryFields(EmbedBuilder embed)
    {
        if (!_entryEnumerator.HasNext)
        {
            embed.AddField("Entries", _noEntriesMessage);
            return;
        }
        var remainingCharacterCount = _maxEmbedLength - embed.Length;
        var fieldCount = (int)Math.Ceiling((double)remainingCharacterCount / EmbedBuilderExtensions.MaxFieldLength);
        var maxFieldCount = Math.Min(_maxFieldCount - embed.Fields.Count, fieldCount);
        
        for (int i = 0; i < maxFieldCount; i++)
        {
            if (!_entryEnumerator.HasNext) break;
            
            var title = $"Entries (Page: {++_entryPages})";
            var limit = _maxEmbedLength - embed.Length - title.Length;
            embed.AddLinedField(title, _entryEnumerator, limit);
        }
    }

    private string GetWinnerLine(RafflePrize prize)
    {
        var winner = prize.Winner;
        var prizePlace = prize.Place.AddPositionalSynonym().PadString(65, 75);
        
        if (winner is not null) return prizePlace + winner.Gamertag;
        if (_config.RollValue is not null && _config.UseCustomEmojis) return prizePlace + _pendingEmoji;
        return prizePlace;
    }
}