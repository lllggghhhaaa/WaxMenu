namespace WaxMenu;


public class MenuIdGenerator(MenuExtensionConfiguration configuration)
{
    public string GenerateId(string menuName, string actionName, params object[] args)
    {
        var id = $"{configuration.Prefix}{configuration.Separator}{menuName}{configuration.Separator}{actionName}";
        
        if (args.Length > 0)
        {
            id += configuration.Separator + string.Join(configuration.Separator, args);
        }

        return id;
    }
}
