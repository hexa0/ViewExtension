using HarmonyLib;
using UnityEngine;
using BepInEx;
using GameNetcodeStuff;

[BepInPlugin("ViewExtension", "ViewExtension", "1.2.0")]
public class ViewExtension : BaseUnityPlugin
{
    private static Harmony _harmony;

    private void Awake()
    {
        _harmony = new Harmony("com.yourname.ViewExtension");
        _harmony.PatchAll();

        // Initialize configuration
        ViewExtensionConfig.Init(Config);

        Logger.LogInfo($"View distance adjusted!");
    }
}

[HarmonyPatch(typeof(PlayerControllerB), "Awake")]
public class CameraFarClipPlanePatch
{
    [HarmonyPostfix]
    public static void Postfix(PlayerControllerB __instance)
    {
        if ((Object)(object)__instance != (Object)null)
        {
            Camera gameplayCamera = __instance.gameplayCamera;
            if ((Object)(object)gameplayCamera != (Object)null)
            {
                // Use the configured value for FarClipPlane
                float newFarClipPlane = ViewExtensionConfig.FarClipPlane.Value;
                gameplayCamera.farClipPlane = newFarClipPlane;
            }
        }
    }
}
