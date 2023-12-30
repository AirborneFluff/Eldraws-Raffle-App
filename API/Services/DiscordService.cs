using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using RaffleApi.Classes;
using RaffleApi.Configurations;
using RaffleApi.Data;
using RaffleApi.Entities;
using RaffleApi.Helpers;
using static RaffleApi.Classes.OperationResult;

namespace RaffleApi.Services;

public sealed class DiscordService : IAsyncDisposable
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly DataContext _context;
    private readonly DiscordSocketClient _discord;
    private readonly string _token;
    private IMessageChannel? _channel;
    private IUserMessage? _message;
    private readonly int _rollPauseDelay = 5000;

    public DiscordService(DiscordSocketClient discord, IConfiguration config, UnitOfWork unitOfWork, ILogger<DiscordService> logger, DataContext context)
    {
        _discord = discord;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _context = context;

        var token = config["DiscordToken"];
        _token = token ?? throw new Exception("Discord Token not defined");
    }

    public async Task<OperationResult> PostRaffle(Raffle raffle, ulong channelId)
    {
        _channel = await GetMessageChannel(channelId);
        if (_channel == null) return FailureResult("Couldn't connect to channel");

        var showWinners = await _unitOfWork.RaffleRepository.HasAnyWinners(raffle.Id);

        var messageFactory = new RaffleMessageFactory(_context, raffle.Id, new RaffleMessageFactoryConfig()
        {
            ShowWinners = showWinners
        });
        await messageFactory.BuildMessages();

        try
        {
            await PostPrimaryMessage(messageFactory.PrimaryMessage, raffle);
            await PostAdditionalMessages(messageFactory.AdditionalMessages, raffle);

            return raffle.DiscordMessageId == null ? FailureResult("Issue sending message") : SuccessResult();
        }
        catch (Exception e)
        {
            return ExceptionResult(e);
        }
    }

    public async Task<OperationResult> SendRoll(Raffle raffle, ulong channelId, int ticketNumber)
    {
        _channel = await GetMessageChannel(channelId);
        if (_channel == null) return FailureResult("Couldn't connect to channel");
        
        var messageFactory = new RaffleMessageFactory(_context, raffle.Id, new RaffleMessageFactoryConfig()
        {
            ShowWinners = true,
            RollValue = ticketNumber
        });
        await messageFactory.BuildPrimaryMessage();

        try
        { 
            await PostPrimaryMessage(messageFactory.PrimaryMessage, raffle);

            return raffle.DiscordMessageId == null ? FailureResult("Issue sending message") : SuccessResult();
        }
        catch (Exception e)
        {
            return ExceptionResult(e);
        }
    }

    private async Task<OperationResult> PostPrimaryMessage(EmbedBuilder embed, Raffle raffle)
    {
        var result = raffle.DiscordMessageId is null
            ? await SendNewMessage(embed)
            : await UpdateMessage(embed, (ulong)raffle.DiscordMessageId);
        
        if (result is null) return FailureResult("Couldn't post message");

        raffle.DiscordMessageId = result.Id;
        if (await _unitOfWork.Complete()) SuccessResult();

        return FailureResult("Couldn't post message");
    }

    private async Task<OperationResult> PostAdditionalMessages(EmbedBuilder[] embeds, Raffle raffle)
    {
        await DeleteMessages(raffle.AdditionalMessageIds.ToArray());
        raffle.AdditionalMessageIds.Clear();
        var tasks = embeds.Select(SendNewMessage);
        
        foreach (var task in tasks)
        {
            var result = await task;
            if (result is null) continue;
            
            raffle.AdditionalMessageIds.Add(result.Id);
        }
        
        if (await _unitOfWork.Complete()) return SuccessResult();

        return FailureResult("Issue posting messages");
    }

    private async Task DeleteMessages(ulong[] messageIds)
    {
        var tasks = messageIds.Select(DeleteMessage);
        await Task.WhenAll(tasks);
    }

    private async Task DeleteMessage(ulong messageId)
    {
        _message ??= await _channel!.GetMessageAsync(messageId) as IUserMessage;
        if (_message is null) return;
        
        await _channel!.DeleteMessageAsync(messageId);
    }

    private async Task<IUserMessage?> SendNewMessage(EmbedBuilder embed)
    {
        return await _channel!.SendMessageAsync("", false, embed.Build());
    }

    private async Task<IUserMessage?> UpdateMessage(EmbedBuilder embed, ulong messageId)
    {
        _message ??= await _channel!.GetMessageAsync(messageId) as IUserMessage;
        if (_message == null) return await SendNewMessage(embed);
        
        await _message.ModifyAsync(messageProperties => messageProperties.Embed = embed.Build());
        return _message;
    }
    
    private async Task<IMessageChannel?> GetMessageChannel(ulong channelId)
    {
        if (_discord.LoginState == LoginState.LoggedOut) 
            await _discord.LoginAsync(TokenType.Bot, _token);
        
        if (_discord.ConnectionState is ConnectionState.Disconnected)
            await _discord.StartAsync();

        return await _discord.GetChannelAsync(channelId) as IMessageChannel;
    }

    public async ValueTask DisposeAsync()
    {
        await _discord.StopAsync();
    }
}