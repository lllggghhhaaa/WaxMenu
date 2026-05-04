using System.Reflection;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.EventArgs;
using WaxMenu.Attributes;
using WaxMenu.Builders;
using WaxMenu.Context;
using WaxMenu.Converters;

namespace WaxMenu;

public class MenuExtension(MenuExtensionConfiguration config, IServiceProvider serviceProvider, MenuIdGenerator generator)
{
    private List<MenuBuilder> _menus = [];
    private Regex _menuRegex = new($"^{config.Prefix}{config.Separator}(?<command>[^{config.Separator}]+){config.Separator}(?<subcommand>[^{config.Separator}]+)(?:{config.Separator}(?<args>.+))*$");

    public void AddMenu<T>()
    {
        var type = typeof(T);
        
        var menuAttribute = type.GetCustomAttribute<MenuAttribute>();
        if (menuAttribute is null) throw new ArgumentException($"The {type.Name} doesn't contains the Menu Attribute");
        
        _menus.Add(new MenuBuilder(menuAttribute, serviceProvider, type));
    }

    public MenuBuilder? GetMenu(string name) => _menus.Find(builder => builder.Name == name);

    public async Task HandleInteractionAsync(DiscordClient sender, ComponentInteractionCreatedEventArgs eventArgs)
    {
        var match = _menuRegex.Match(eventArgs.Id);
        
        if (!match.Success) return;

        var command = match.Groups["command"].Value;
        var subcommand = match.Groups["subcommand"].Value;
        var args = match.Groups["args"].Value.Split(config.Separator, StringSplitOptions.RemoveEmptyEntries);
        
        var menuBuilder = GetMenu(command) ?? throw new NullReferenceException($"Cannot find the menu {command}");
        var subMenuBuilder = menuBuilder.GetSubMenu(subcommand) ?? throw new NullReferenceException($"cannot find the submenu {subcommand}");
        
        var parameters = new object[args.Length + 1];
        parameters[0] = new MenuContext(sender, eventArgs, generator, command);

        var tasks = new Task<object>[args.Length];
        for (var i = 0; i < args.Length; i++)
        {
            if (i >= subMenuBuilder.ArgsType.Length) break;
            
            if (!TypeConverter.Converters.ContainsKey(subMenuBuilder.ArgsType[i]))
                throw new InvalidOperationException($"The type {subMenuBuilder.ArgsType[i].Name} is not implemented on menu parameter type converter");
            
            tasks[i] = TypeConverter.Converters[subMenuBuilder.ArgsType[i]]
                .Invoke(new ConversionContext(sender, eventArgs, args[i]));
        }

        await Task.WhenAll(tasks);

        for (var i = 0; i < tasks.Length; i++)
        {
            parameters[i + 1] = tasks[i].Result;
        }

        var result = subMenuBuilder.Method.Invoke(menuBuilder.Instance, parameters);
        if (result is Task task) await task;
        else if (result is ValueTask valueTask) await valueTask;
    }
}