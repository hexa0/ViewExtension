using BepInEx.Configuration;

public static class ViewExtensionConfig
{
    public static ConfigEntry<float> FarClipPlane;
    public static ConfigEntry<bool> MenuOption { get; set; }
    public static ConfigEntry<bool> DebugLogging { get; set; }
    internal static ConfigFile Config { get; private set; }

    internal static void Init(ConfigFile config)
    {
        Config = config;
        FarClipPlane = config.Bind("1. General", "FarClipPlane", 1000f, "View distance in units. Vanilla is 400. (between 1 and 10000)");
        MenuOption = config.Bind("1. General", "MenuOption", true, "Whether to add an option in the settings menu to change the view distance.");
        DebugLogging = config.Bind("2. Debugging", "DebugLogging", false, "Whether to display debug messages.");
    }

    public static void Save()
    {
        Config.Save();
    }
}
