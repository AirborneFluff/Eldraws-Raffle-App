using Discord;
using Discord.WebSocket;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Services;

public sealed class DiscordService
{
    private readonly DiscordSocketClient _discord;
    private readonly string _token;
    private IMessageChannel? _channel;

    public DiscordService(DiscordSocketClient discord, IConfiguration config)
    {
        _discord = discord;

        var token = config["DiscordToken"];
        _token = token ?? throw new Exception("Discord Token not defined");
    }

    public async Task<ulong?> SendRaffleEmbed(Raffle raffle, ulong channelId)
    {
        await StartSession(channelId);
        if (_channel == null) return null;
        
        var embed = raffle.GenerateEmbed();
        ulong? messageId = null;

        if (raffle.DiscordMessageId != null)
        {
            messageId = await EditMessage(embed, (ulong)raffle.DiscordMessageId);
            if (messageId != null) return await Complete(messageId);
        }
        
        messageId = await SendNewMessage(embed);
        return await Complete(messageId);
    }

    private async Task<ulong?> Complete(ulong? value)
    {
        await CloseSession();
        return value;
    }

    private async Task<ulong?> SendNewMessage(EmbedBuilder embed)
    {
        if (_channel == null) return await Complete(null);
        
        var msgResult = await _channel.SendMessageAsync("", false, embed.Build());
        return msgResult?.Id;
    }

    private async Task<ulong?> EditMessage(EmbedBuilder embed, ulong messageId)
    {
        if (_channel == null) return await Complete(null);

        if (!(await _channel.GetMessageAsync(messageId) is IUserMessage msg)) return await Complete(null);
        await msg.ModifyAsync(messageProperties => messageProperties.Embed = embed.Build());
        
        return messageId;
    }
    
    private async Task StartSession(ulong channelId)
    {
        if (_discord.LoginState == LoginState.LoggedOut) 
            await _discord.LoginAsync(TokenType.Bot, _token);
        
        await _discord.StartAsync();

        _channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
    }

    private async Task CloseSession()
    {
        await _discord.StopAsync();
    }
}