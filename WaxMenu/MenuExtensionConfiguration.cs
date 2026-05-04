namespace WaxMenu;

public record struct MenuExtensionConfiguration(string Prefix = "menu", string Separator = "_")
{
    public MenuExtensionConfiguration() : this("menu", "_") { }
}