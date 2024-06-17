using HarmonyLib;
using UnityEngine;
using BepInEx;
using GameNetcodeStuff;
using UnityEngine.UIElements;

[BepInPlugin("ViewExtension", "ViewExtension", "1.2.3")]
public class ViewExtensionLoader : BaseUnityPlugin
{
    public static float tempValue;

    private void Awake()
    {
         Harmony _harmony = new("com.yourname.ViewExtension");
        _harmony.PatchAll();

        ViewExtensionConfig.Init(Config);

        if (float.IsNaN(ViewExtensionConfig.FarClipPlane.Value) || ViewExtensionConfig.FarClipPlane.Value <= 0 || ViewExtensionConfig.FarClipPlane.Value >= 100000)
        {
            Logger.LogWarning("Invalid value for FarClipPlane in the config file (" + ViewExtensionConfig.FarClipPlane.Value + "). Using default value of 1500.");

            tempValue = 1500f;
        }
        else
        {
            tempValue = ViewExtensionConfig.FarClipPlane.Value;
        }

        Logger.LogInfo($"View distance adjusted to " + tempValue);
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
                float newFarClipPlane = ViewExtensionLoader.tempValue;
                gameplayCamera.farClipPlane = newFarClipPlane;
            }
        }
    }
}
