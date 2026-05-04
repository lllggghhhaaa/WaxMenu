using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using WaxMenu.Attributes;
using WaxMenu.Context;
using WaxMenu.Sample.Menu;

namespace WaxMenu.Sample.Commands;

[Command("menu"), Menu("guild")]
public class MenuCommands
{
    [Command("open"), RequireGuild]
    public async ValueTask Menu(CommandContext context)
    {
        await context.RespondAsync(GuildMenuBuilder.MainMenu(context.Guild));
    }

    [MenuAction("home")]
    public async ValueTask Menu(MenuContext context)
    {
        await context.EditResponse(GuildMenuBuilder.MainMenu(context.Guild));
    }

    [MenuAction("members")]
    public async ValueTask Members(MenuContext context)
    {
        await context.EditResponse(GuildMenuBuilder.MembersMenu());
    }
}