## Introduction
WaxMenu is a library to create interactive menus and component tasks with [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)

https://www.nuget.org/packages/WaxMenu

## How to use

### Setup
````csharp
// With DiscordClientBuilder
builder.UseWaxMenu(extension =>
    {
        // Add a menu to be used
        extension.AddMenu<GuildCommand>();
    }, new MenuExtensionConfiguration("menu", "_"));
````

````csharp
// With ServiceProvider
builder.Services
    // ...
    .AddMenuExtension(extension =>
    {
        // Add a menu to be used
        extension.AddMenu<GuildCommand>();
    }, new MenuExtensionConfiguration("menu", "_"));
````

### Adding a menu
You need to add a menu attribute to the class that you want to make the menu. You can use with Command extensions too
<br/>Inside the class, you can add Menu Actions. This actions are triggered in every component interaction where match the requisites (See [ID Pattern](#id-pattern))


#### Command
````csharp
[Command("menu"), Menu("guild")]
public class MenuCommands(MenuIdGenerator generator)
{
    [Command("open"), RequireGuild]
    public async ValueTask Menu(CommandContext context)
    {
        await context.RespondAsync(GuildMenuBuilder.MainMenu(context.Guild, generator));
    }

    [MenuAction("home")]
    public async ValueTask Menu(MenuContext context)
    {
        await context.EditResponse(GuildMenuBuilder.MainMenu(context.Guild, generator));
    }

    [MenuAction("members")]
    public async ValueTask Members(MenuContext context)
    {
        await context.EditResponse(GuildMenuBuilder.MembersMenu(generator));
    }
}
````

#### Builder
````csharp
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
````

### ID Pattern
The component CustomId should match this pattern
``prefix_command_subcommand_param1_param2``
- `prefix`* The prefix that you added on configuration
- `_`* The separator that you added on configuration
- `command`* Is the name of the menu (`[Menu("guild")]`)
- `subcommand`* Is the name of the action (`[MenuAction("home")]`)
- `params`*(optional)* Are parameters that is passed through the method (See [Action Parameters](#action-parameters))

````csharp
// Example
messageBuilder.AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Secondary, "menu_guild_home", "Home"));
````

### Action Parameters

| Type           | Format          | Note |
|----------------|-----------------|------|
| bool           | `true`/`false`  |      |
| byte           | `0` - `255`     |      |
| sbyte          | `-128` - `127`  |      |
| char           | `c`             |      |
| decimal        | `547589.43642`  |      | 
| double         | `325436.4535`   |      |
| float          | `3524.56`       |      | 
| int            | `-352456`       |      |
| uint           | `246356`        |      |
| long           | `-32534456254`  |      |
| ulong          | `352434436346`  |      |
| short          | `-2356`         |      |
| ushort         | `4367`          |      |
| string         | `text`          |      |
| DiscordChannel | `3253423463456` | ID   |
| DiscordUser    | `4364536534623` | ID   |
| DiscordGuild   | `5473536453632` | ID   |


````csharp
// Action example with parameters
// menu_guild_test_channelid
[MenuAction("test")]
public async ValueTask Test(MenuContext context, DiscordChannel channel)
{
    Console.WriteLine(channel.Name);     
}
````
