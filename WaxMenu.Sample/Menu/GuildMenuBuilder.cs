using DSharpPlus.Entities;

namespace WaxMenu.Sample.Menu;

public static class GuildMenuBuilder
{
    private const string Prefix = "menu";
    private const string MenuName = "guild";
    
    public static DiscordMessageBuilder MainMenu(DiscordGuild guild)
    {
        var embed = new DiscordEmbedBuilder()
            .WithTitle($"Menu - {guild.Name}")
            .WithDescription("Guild information...")
            .WithColor(DiscordColor.Blue)
            .AddField("Server", guild.Name, false)
            .AddField("Members", guild.MemberCount.ToString(), false);

        var messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed)
            .AddActionRowComponent(
                new DiscordButtonComponent(
                    DiscordButtonStyle.Secondary,
                    $"{Prefix}_{MenuName}_members",
                    "Members"));

        return messageBuilder;
    }

    public static DiscordMessageBuilder MembersMenu()
    {
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Members")
            .WithDescription("Member list...")
            .WithColor(DiscordColor.Green);

        var messageBuilder = new DiscordMessageBuilder()
            .AddEmbed(embed)
            .AddActionRowComponent(
                new DiscordButtonComponent(
                    DiscordButtonStyle.Secondary,
                    $"{Prefix}_{MenuName}_home",
                    "Back to Home"));

        return messageBuilder;
    }
}

