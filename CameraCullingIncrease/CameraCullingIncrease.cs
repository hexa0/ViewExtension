using HarmonyLib;
using UnityEngine;
using BepInEx;
using GameNetcodeStuff;

namespace CameraCullingIncrease
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class CameraCullingIncrease : BaseUnityPlugin
    {
        private static readonly Harmony Harmony = new(PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            Logger.LogInfo($"Applying patch...");
            Harmony.PatchAll();
            Logger.LogInfo($"View distance adjusted!");
        }
    }

    [HarmonyPatch(typeof(PlayerControllerB), "Awake")]
    public class CameraFarClipPlanePatch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayerControllerB __instance)
        {
            if (__instance != null)
            {
                Camera gameplayCamera = __instance.gameplayCamera;

                if (gameplayCamera != null)
                {
                    float newFarClipPlane = 1000f;
                    gameplayCamera.farClipPlane = newFarClipPlane;
                }
            }
        }
    }
}
