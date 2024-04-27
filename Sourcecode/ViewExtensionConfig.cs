using UnityEngine;
using BepInEx.Configuration;

public static class ViewExtensionConfig
{
    // Define a configuration option for FarClipPlane
    public static ConfigEntry<float> FarClipPlane;

    internal static void Init(ConfigFile config)
    {
        // Define the configuration option for FarClipPlane with a default value of 1500f
        FarClipPlane = config.Bind("General", "FarClipPlane", 1500f, "View distance in units. Vanilla is 400.");

        if (float.IsNaN(FarClipPlane.Value) || FarClipPlane.Value <= 0 || FarClipPlane.Value >= 100000)
        {
            // If so, log a warning and set a default value
            Debug.LogWarning("ViewExtension: Invalid value for FarClipPlane in the config file. Using default value of 1500.");
            FarClipPlane.Value = 1500f; // Set a default value
        }
    }
}
