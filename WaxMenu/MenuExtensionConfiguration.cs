namespace WaxMenu;

public record MenuExtensionConfiguration(string Prefix = "menu", string Separator = "_")
{
    public MenuExtensionConfiguration() : this("menu", "_") { }
}