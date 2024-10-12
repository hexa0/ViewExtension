using HarmonyLib;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;

[BepInPlugin("ViewExtension", "ViewExtension", "1.2.0")]
public class ViewExtension : BaseUnityPlugin
{
    private static Harmony harmony;
    public static ManualLogSource logger;

    private void Awake()
    {
        logger = Logger;

        harmony = new Harmony("com.yourname.ViewExtension");
        harmony.PatchAll();

        // Initialize configuration
        ViewExtensionConfig.Init(Config);
    }
}

[HarmonyPatch(typeof(StartOfRound), "Awake")]
public class CameraFarClipPlanePatch
{
    [HarmonyPatch("SwitchCamera")]
    [HarmonyPrefix]
    static void SwitchCameraFarClipPlanePatch(Camera newCamera)
    {
        ViewExtension.logger.LogDebug($"SwitchCamera Called with '{newCamera.name}', updating farClipPlane to {ViewExtensionConfig.FarClipPlane.Value}");
        newCamera.farClipPlane = ViewExtensionConfig.FarClipPlane.Value;
    }

}