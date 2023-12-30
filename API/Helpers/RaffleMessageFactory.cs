using Discord;
using Microsoft.EntityFrameworkCore;
using RaffleApi.Configurations;
using RaffleApi.Data;
using RaffleApi.Entities;
using RaffleApi.Extensions;
using EmbedBuilderExtensions = RaffleApi.Extensions.EmbedBuilderExtensions;

namespace RaffleApi.Helpers;

public class RaffleMessageFactory
{
    private readonly DataContext _context;
    private readonly int _raffleId;
    private readonly RaffleMessageFactoryConfig _config;
    private readonly LookAheadEnumerator<string> _entryEnumerator;
    private readonly IQueryable<RaffleEntry> _entries;
    private readonly IQueryable<RafflePrize> _prizes;
    private Raffle _raffle = null!;
    
    private readonly int _maxEmbedLength = 6000;
    private readonly int _maxTitleLength = 256;
    private readonly int _maxFieldCount = 25;

    private readonly string _noPrizesMessage = "No prizes have been listed yet";
    private readonly string _noEntriesMessage = "No entries have been made yet";
    private readonly string _pendingEmoji = "<a:threepointsanima:1005525060490117191>";
    private readonly string _diceEmoji = "<a:dices:1172979983321411676>";

    private int _entryPages = 0;

    private IList<EmbedBuilder> _messages = new List<EmbedBuilder>();
    public EmbedBuilder PrimaryMessage => _messages.First();
    public EmbedBuilder[] AdditionalMessages => _messages.Skip(1).ToArray();
    
    public RaffleMessageFactory(DataContext context, int raffleId, RaffleMessageFactoryConfig config)
    {
        _config = config;
        _context = context;
        _raffleId = raffleId;

        _entries = context.Entries
            .Where(entry => entry.RaffleId == raffleId)
            .Where(entry => entry.LowTicket != 0)
            .OrderBy(entry => entry.LowTicket);

        _prizes = context.Prizes
            .Where(prize => prize.RaffleId == raffleId)
            .OrderBy(prize => prize.Place);
        
        _entryEnumerator = new LookAheadEnumerator<string>(_entries.Select(entry => entry.ToString()).GetEnumerator());
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
        
        if (_config.RollValue is not null) AddRollingField(embed, (int)_config.RollValue);
        if (_config.ShowWinners) await AddWinnersField(embed);
        AddPrizesField(embed);
        AddEntryFields(embed);
        return embed;
    }

    private async Task AddWinnersField(EmbedBuilder embed)
    {
        var prizes = await _prizes.ToListAsync();
        
        if (!prizes.Any())
        {
            embed.AddField("Prizes", _noPrizesMessage);
            return;
        }

        var winnersList = new List<string>();
        var tasks = prizes.Select(async prize => await GetWinnerLine(prize));
        foreach (var task in tasks)
        {
            var result = await task;
            winnersList.Add(result);
        }
        embed.AddLinedField("Winners", winnersList.GetEnumerator());
    }

    private void AddRollingField(EmbedBuilder embed, int value)
    {
        embed.AddField("Roll", "Rolling... " + _diceEmoji + value);
    }

    private void AddPrizesField(EmbedBuilder embed)
    {
        var prizes = _raffle.Prizes;
        if (!prizes.Any())
        {
            embed.AddField("Prizes", _noPrizesMessage);
            return;
        }

        using var enumerator = prizes.Select(prize => prize.ToString()).GetEnumerator();
        embed.AddLinedField("Prizes", enumerator);
    }

    private void AddEntryFields(EmbedBuilder embed)
    {
        if (!_entryEnumerator.HasNext) return;
        
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

    private async Task<string> GetWinnerLine(RafflePrize prize)
    {
        var winner = await _context.Entrants.FirstOrDefaultAsync(entrant => entrant.Id == prize.WinnerId);
        var prizePlace = prize.Place.AddPositionalSynonym().PadString(65, 75);
        
        if (winner is not null) return prizePlace + winner.Gamertag;
        if (_config.RollValue is not null) return prizePlace + _pendingEmoji;
        return prizePlace;
    }
}