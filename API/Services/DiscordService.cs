using Discord;
using Discord.WebSocket;
using RaffleApi.Classes;
using RaffleApi.Data;
using RaffleApi.Entities;
using RaffleApi.Helpers;
using static RaffleApi.Classes.OperationResult;

namespace RaffleApi.Services;

public sealed class DiscordService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly RandomService _random;
    private readonly DiscordSocketClient _discord;
    private readonly string _token;

    public DiscordService(DiscordSocketClient discord, IConfiguration config, UnitOfWork unitOfWork, RandomService random)
    {
        _discord = discord;
        _unitOfWork = unitOfWork;
        _random = random;

        var token = config["DiscordToken"];
        _token = token ?? throw new Exception("Discord Token not defined");
    }

    public async Task<OperationResult> SendRaffleEmbed(Raffle raffle, ulong channelId)
    {
        var channel = await GetMessageChannel(channelId);
        if (channel == null) return FailureResult("Couldn't connect to channel");

        var embed = raffle.GenerateEmbed();

        try
        { 
            var result = await PostMessage(channel, embed, raffle);
            if (result.Failure) return result;

            return raffle.DiscordMessageId == null ? FailureResult("Issue sending message") : SuccessResult();
        }
        catch (Exception e)
        {
            return ExceptionResult(e);
        }
    }

    private async Task<OperationResult> PostMessage(IMessageChannel channel, EmbedBuilder embed, Raffle raffle)
    {
        var messageId = raffle.DiscordMessageId;
        if (messageId == null) return await SendNewMessage(channel, embed, raffle);

        return await UpdateMessage(channel, embed, raffle);
    }

    private async Task<OperationResult> SendNewMessage(IMessageChannel channel, EmbedBuilder embed, Raffle raffle)
    {
        var msgResult = await channel.SendMessageAsync("", false, embed.Build());
        if (msgResult == null) return FailureResult("Couldn't send message");

        raffle.DiscordMessageId = msgResult.Id;
        if (await _unitOfWork.Complete()) return SuccessResult();
        
        return FailureResult("Issue setting message Id");
    }

    private async Task<OperationResult> UpdateMessage(IMessageChannel channel, EmbedBuilder embed, Raffle raffle)
    {
        if (raffle.DiscordMessageId == null) throw new Exception("MessageId cannot be null");
        
        var message = await channel.GetMessageAsync((ulong) raffle.DiscordMessageId) as IUserMessage;
        if (message == null) return await SendNewMessage(channel, embed, raffle);
        
        await message.ModifyAsync(messageProperties => messageProperties.Embed = embed.Build());
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

    // public async Task RollWinners(Raffle raffle, ulong channelId)
    // {
    //     if (raffle.DiscordMessageId == null) return await Complete(null);
    //     
    //     
    // }
    //
    public async Task<OperationResult> RollNextWinner(Raffle raffle, ulong channelId)
    {
        if (raffle.DiscordMessageId == null) return FailureResult("Raffle hasn't been posted to Discord");
        
        var channel = await GetMessageChannel(channelId);
        if (channel == null) return FailureResult("Couldn't connect to channel");
    
        var prize = raffle.Prizes
            .OrderByDescending(p => p.Place)
            .FirstOrDefault(p => p.WinningTicketNumber == null);

        if (prize == null) return SuccessResult();
        
        var max = raffle.Entries.Select(e => e.Tickets.Item2).Max();
        var result = await _random.GetRandomInt(max, 1);

        prize.WinningTicketNumber = result;
        if (!await _unitOfWork.Complete()) return FailureResult("Issue setting prize winner");
    
        var embed = raffle.GenerateEmbed(true);
        return await UpdateMessage(channel, embed, raffle);
    }
}