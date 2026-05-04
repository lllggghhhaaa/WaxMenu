using DSharpPlus;

namespace WaxMenu.Sample;

public class Worker(ILogger<Worker> logger, DiscordClient discordClient) : IHostedService
{
    public async Task StartAsync(CancellationToken token)
    {
        await discordClient.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken token)
    {
        await discordClient.DisconnectAsync();
    }
}