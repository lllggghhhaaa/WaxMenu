using DSharpPlus.Entities;

namespace WaxMenu.Sample.Menu;

public static class GuildMenuBuilder
{
    public static DiscordMessageBuilder MainMenu(DiscordGuild guild, MenuIdGenerator generator)
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
                    generator.GenerateId("guild", "members"),
                    "Members"));

        return messageBuilder;
    }

    public static DiscordMessageBuilder MembersMenu(MenuIdGenerator generator)
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
                    generator.GenerateId("guild", "home"),
                    "Back to Home"));

        return messageBuilder;
    }
}
