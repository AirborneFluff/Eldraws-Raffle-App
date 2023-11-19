using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using RaffleApi.Classes;
using RaffleApi.Data;
using RaffleApi.Entities;
using RaffleApi.Helpers;
using static RaffleApi.Classes.OperationResult;

namespace RaffleApi.Services;

public sealed class DiscordService : IAsyncDisposable
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly DiscordSocketClient _discord;
    private readonly string _token;
    private IMessageChannel? _channel;
    private IUserMessage? _message;
    private readonly int _rollPauseDelay = 5000;

    public DiscordService(DiscordSocketClient discord, IConfiguration config, UnitOfWork unitOfWork, ILogger<DiscordService> logger)
    {
        _discord = discord;
        _unitOfWork = unitOfWork;
        _logger = logger;

        var token = config["DiscordToken"];
        _token = token ?? throw new Exception("Discord Token not defined");
    }

    public async Task<OperationResult> PostRaffle(Raffle raffle, ulong channelId)
    {
        _channel = await GetMessageChannel(channelId);
        if (_channel == null) return FailureResult("Couldn't connect to channel");

        var embed = raffle.GenerateEmbed();

        try
        { 
            var result = await PostMessage(embed, raffle);
            if (result.Failure) return result;

            return raffle.DiscordMessageId == null ? FailureResult("Issue sending message") : SuccessResult();
        }
        catch (Exception e)
        {
            return ExceptionResult(e);
        }
    }

    public async Task<OperationResult> RollWinners(Raffle raffle, ulong channelId, RaffleDrawParams options)
    {
        if (raffle.DiscordMessageId == null) return FailureResult("Raffle hasn't been posted to Discord");
        
        _channel = await GetMessageChannel(channelId);
        if (_channel == null) return FailureResult("Couldn't connect to channel");

        var embed = raffle.GenerateRollingEmbed();
        var updateResult = await UpdateMessage(embed, raffle);
        if (updateResult.Failure) return updateResult;

        return await Task.Run(() => RollAllWinners(raffle, options));
    }

    public async Task<OperationResult> SendRoll(Raffle raffle, ulong channelId, int ticketNumber)
    {
        _channel = await GetMessageChannel(channelId);
        if (_channel == null) return FailureResult("Couldn't connect to channel");

        var embed = raffle.GenerateRollingEmbed(ticketNumber);

        try
        { 
            var result = await UpdateMessage(embed, raffle);
            if (result.Failure) return result;

            return raffle.DiscordMessageId == null ? FailureResult("Issue sending message") : SuccessResult();
        }
        catch (Exception e)
        {
            return ExceptionResult(e);
        }
    }

    private async Task<OperationResult> PostMessage(EmbedBuilder embed, Raffle raffle)
    {
        if (raffle.DiscordMessageId == null) return await SendNewMessage(embed, raffle);
        return await UpdateMessage(embed, raffle);
    }

    private async Task<OperationResult> SendNewMessage(EmbedBuilder embed, Raffle raffle)
    {
        if (_channel == null) throw new Exception("Discord channel not set");
        
        _message = await _channel.SendMessageAsync("", false, embed.Build());
        if (_message == null) return FailureResult("Couldn't send message");

        raffle.DiscordMessageId = _message.Id;
        if (await _unitOfWork.Complete()) return SuccessResult();
        
        return FailureResult("Issue setting message Id");
    }

    private async Task<OperationResult> UpdateMessage(EmbedBuilder embed, Raffle raffle)
    {
        if (raffle.DiscordMessageId == null) throw new Exception("Discord message Id not set");
        if (_channel == null) throw new Exception("Channel not set");
        
        _message ??= await _channel.GetMessageAsync((ulong) raffle.DiscordMessageId) as IUserMessage;
        if (_message == null) return await SendNewMessage(embed, raffle);
        await _message.ModifyAsync(messageProperties => messageProperties.Embed = embed.Build());
        
        return SuccessResult();
    }
    
    private async Task<IMessageChannel?> GetMessageChannel(ulong channelId)
    {
        if (_discord.LoginState == LoginState.LoggedOut) 
            await _discord.LoginAsync(TokenType.Bot, _token);
        
        if (_discord.ConnectionState is ConnectionState.Disconnected)
            await _discord.StartAsync();

        return await _discord.GetChannelAsync(channelId) as IMessageChannel;
    }

    private async Task<OperationResult> RollAllWinners(Raffle raffle, RaffleDrawParams options)
    {
        for (var i = 0; i < raffle.Prizes.Count; i++)
        {
            var isLast = i == raffle.Prizes.Count - 1;

            var watch = new Stopwatch();
            watch.Start();
            var result = await RollNextWinner(raffle, options);
            if (result.Failure) return FailureResult("Issue rolling winner");
            if (result.Value == null) continue;
            
            var requiredDelay = options.Delay * 1000 - (int)watch.ElapsedMilliseconds;
            if (!isLast && requiredDelay > 0) continue;

            await Task.Delay(requiredDelay);
            
            _logger.Log(LogLevel.Information, watch.ElapsedMilliseconds.ToString());
        }

        var embed = raffle.GenerateEmbed();
        return await UpdateMessage(embed, raffle);
    }
    
    private async Task<OperationResult> RollNextWinner(Raffle raffle, RaffleDrawParams options)
    {
        var prize = raffle.Prizes
            .OrderBy(p => p.Place)
            .FirstOrDefault(p => p.WinningTicketNumber == null);

        if (prize == null) return SuccessResult(null);
        
        prize.WinningTicketNumber = await RollTicket(raffle, options);
        await _unitOfWork.Complete();
    
        var embed = raffle.GenerateRollingEmbed();
        return await UpdateMessage(embed, raffle);
    }

    private async Task<int> RollTicket(Raffle raffle, RaffleDrawParams options)
    {
        
        var max = raffle.Entries.Select(e => e.Tickets.Item2).Max();
        var tickets = RandomService.GetRandomIntegerList(max, 1, options.MaxRerolls);

        int? validTicket = null;
        var lastRolledTicket = tickets[0];
        var reroll = false;
        foreach (var t in tickets)
        {
            var winner = GetWinnerFromTicket(raffle, t);
            lastRolledTicket = t;
            
            var embed = raffle.GenerateRollingEmbed(t, reroll);
            
            var watch = new Stopwatch();
            watch.Start();
            await UpdateMessage(embed, raffle);
            
            var requiredDelay = _rollPauseDelay - (int)watch.ElapsedMilliseconds;
            if (requiredDelay > 0) await Task.Delay(requiredDelay);

            if (options.PreventMultipleWins && HasEntrantAlreadyWon(raffle, winner))
            {
                reroll = true;
                continue;
            }

            validTicket = t;
            break;
        }

        return validTicket ?? lastRolledTicket;
    }

    private Entrant GetWinnerFromTicket(Raffle raffle, int ticketNumber)
    {
        var winner = raffle.Entries.FirstOrDefault(e => e.Tickets.Item1 <= ticketNumber && e.Tickets.Item2 >= ticketNumber);
        var entrant = winner == null ? raffle.Entries.First().Entrant : winner.Entrant;
        if (entrant == null) throw new Exception("Raffle entries not included");

        return entrant;
    }

    private bool HasEntrantAlreadyWon(Raffle raffle, Entrant entrant)
    {
        foreach (var prize in raffle.Prizes)
        {
            if (prize.WinningTicketNumber == null) continue;
            var winner = GetWinnerFromTicket(raffle, (int) prize.WinningTicketNumber);
            
            if (winner.Id == entrant.Id) return true;
        }

        return false;
    }

    public async ValueTask DisposeAsync()
    {
        await _discord.StopAsync();
    }
}