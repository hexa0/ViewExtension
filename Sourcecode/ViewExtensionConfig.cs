using UnityEngine;
using BepInEx.Configuration;

public static class ViewExtensionConfig
{
    public static ConfigEntry<float> FarClipPlane;

    internal static void Init(ConfigFile config)
    {
        FarClipPlane = config.Bind("General", "FarClipPlane", 1500f, "View distance in units. Vanilla is 400. (between 1 and 99999)");
    }
}
