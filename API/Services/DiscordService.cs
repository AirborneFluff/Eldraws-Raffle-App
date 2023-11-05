using Discord;
using Discord.WebSocket;
using RaffleApi.Entities;
using RaffleApi.Helpers;

namespace RaffleApi.Services;

public sealed class DiscordService
{
    private readonly DiscordSocketClient _discord;
    private readonly string _token;

    public DiscordService(DiscordSocketClient discord, IConfiguration config)
    {
        _discord = discord;

        var token = config["DiscordToken"];
        _token = token ?? throw new Exception("Discord Token not defined");
    }

    public async Task<ulong?> SendRaffleEmbed(Raffle raffle, ulong channelId)
    {
        var channel = await StartSession(channelId);
        if (channel == null) return await Complete(null);
        
        var embed = raffle.GenerateEmbed();
        ulong? messageId = null;

        if (raffle.DiscordMessageId != null)
        {
            messageId = await EditMessage(channel, embed, (ulong)raffle.DiscordMessageId);
            if (messageId != null) return await Complete(messageId);
        }
        
        messageId = await SendNewMessage(channel, embed);
        return await Complete(messageId);
    }

    private async Task<ulong?> Complete(ulong? value)
    {
        await _discord.StopAsync();
        return value;
    }

    private async Task<ulong?> SendNewMessage(IMessageChannel channel, EmbedBuilder embed)
    {
        var msgResult = await channel.SendMessageAsync("", false, embed.Build());
        return msgResult?.Id;
    }

    private async Task<ulong?> EditMessage(IMessageChannel channel, EmbedBuilder embed, ulong messageId)
    {
        if (await channel.GetMessageAsync(messageId) is not IUserMessage msg) return await Complete(null);
        await msg.ModifyAsync(messageProperties => messageProperties.Embed = embed.Build());
        
        return messageId;
    }
    
    private async Task<IMessageChannel?> StartSession(ulong channelId)
    {
        if (_discord.LoginState == LoginState.LoggedOut) 
            await _discord.LoginAsync(TokenType.Bot, _token);
        
        await _discord.StartAsync();

        return await _discord.GetChannelAsync(channelId) as IMessageChannel;
    }
}