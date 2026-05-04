using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Extensions;
using WaxMenu;
using WaxMenu.Sample;
using WaxMenu.Sample.Commands;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddShardedDiscordClient(
    builder.Configuration["Discord:Token"] ?? throw new InvalidOperationException("Discord token not found in configuration"),
    DiscordIntents.All
    );
builder.Services.AddCommandsExtension((_, extension) =>
{
    extension.AddCommands<MenuCommands>();
});
builder.Services.AddMenuExtension(extension =>
{
    extension.AddMenu<MenuCommands>();
}, new MenuExtensionConfiguration());

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();