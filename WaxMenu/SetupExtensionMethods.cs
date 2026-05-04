using DSharpPlus;
using DSharpPlus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using WaxMenu.Events;

namespace WaxMenu;

public static class SetupExtensionMethods
{
    public static DiscordClientBuilder UseWaxMenu(this DiscordClientBuilder builder, Action<MenuExtension> menuCallback,
        MenuExtensionConfiguration configuration) =>
        builder.ConfigureServices(services => services.AddMenuExtension(menuCallback, configuration));

    public static IServiceCollection AddMenuExtension(this IServiceCollection services,
        Action<MenuExtension> menuCallback, MenuExtensionConfiguration configuration)
    {
        services.ConfigureEventHandlers(builder => builder.AddEventHandlers<ComponentInteractionCreated>());
        services.AddSingleton(configuration);
        services.AddSingleton<MenuIdGenerator>();
        services.AddSingleton<MenuExtension>(provider =>
        {
            DiscordClient client = provider.GetRequiredService<DiscordClient>();
            var generator = provider.GetRequiredService<MenuIdGenerator>();
            var menu = new MenuExtension(configuration, client.ServiceProvider, generator);

            menuCallback(menu);
            return menu;
        });

        return services;
    }
}