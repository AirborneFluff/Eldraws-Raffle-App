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
        this._discord = discord;

        var token = config["DiscordToken"];
        this._token = token ?? throw new Exception("Discord Token not defined");
    }

    public async Task SendRaffleEmbed(Raffle raffle, ulong channelId)
    {
        await StartSession(channelId);
        if (_channel == null) return;
        
        var embed = raffle.GenerateEmbed();
        
        var msgResult = await _channel.SendMessageAsync("", false, embed.Build());
        raffle.DiscordMessageId = msgResult.Id;

        //
        // if (raffle.DiscordMessageId != null)
        // {
        //     var msg = await _channel.GetMessageAsync((ulong)raffle.DiscordMessageId) as IUserMessage;
        //     if (msg != null)
        //     {
        //         await msg.ModifyAsync(msg => msg.Embed = embed.Build());
        //         goto Finish;
        //     }
        // }
        //
        // var msgResult = await _channel.SendMessageAsync("", false, embed.Build());
        // raffle.DiscordMessageId = msgResult.Id;
        //
        // Finish:
        await CloseSession();
        // return raffle.DiscordMessageId;
    }
    
    private async Task StartSession(ulong channelId)
    {
        await _discord.LoginAsync(TokenType.Bot, _token);
        await _discord.StartAsync();

        _channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
    }

    private async Task CloseSession()
    {
        await _discord.LogoutAsync();
    }
}