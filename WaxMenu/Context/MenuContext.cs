using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace WaxMenu.Context;

public class MenuContext(DiscordClient client, ComponentInteractionCreatedEventArgs args, MenuIdGenerator generator, string menuName)
{
    public DiscordClient Client = client;
    public DiscordInteraction Interaction = args.Interaction;
    public string InteractionId = args.Id;
    public DiscordUser User = args.User;
    public DiscordGuild Guild = args.Guild;
    public DiscordChannel Channel = args.Channel;
    public string[] SelectValues = args.Values;
    public DiscordMessage Message = args.Message;
    public string Locale = args.Locale;
    public string GuildLocale = args.GuildLocale;
    public string MenuName = menuName;

    public string GenerateId(string actionName, params object[] args) =>
        generator.GenerateId(MenuName, actionName, args);

    public string GenerateId(string menuName, string actionName, params object[] args) =>
        generator.GenerateId(menuName, actionName, args);

    public async Task EditResponse(DiscordMessageBuilder messageBuilder) =>
        await Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(messageBuilder));
}