using HarmonyLib;
using UnityEngine;
using BepInEx;
using GameNetcodeStuff;
using System.IO;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

[BepInPlugin("ViewExtension", "ViewExtension", "1.3.0")]
public class ViewExtensionLoader : BaseUnityPlugin
{
    private static string assetBundleName = "viewextensionassetbundle";
    private static AssetBundle assetBundle;
    private static GameObject settingsObject;

    public static bool isMainMenu;

    private void Awake()
    {
        ViewExtensionConfig.Init(Config);

        if (ViewExtensionConfig.DebugLogging.Value) Logger.LogInfo($"MenuOption is set to " + ViewExtensionConfig.MenuOption.Value);
        if (ViewExtensionConfig.MenuOption.Value) LoadAssetBundle();

        if (float.IsNaN(ViewExtensionConfig.FarClipPlane.Value) ||
            ViewExtensionConfig.FarClipPlane.Value < 1 ||
            ViewExtensionConfig.FarClipPlane.Value > 10000)
        {
            Logger.LogWarning("Invalid value for FarClipPlane in the config file (" + ViewExtensionConfig.FarClipPlane.Value + "). Using default value of 1000.");
            ViewExtensionConfig.FarClipPlane.Value = 1000f;
        }

        Logger.LogInfo($"View distance adjusted to " + ViewExtensionConfig.FarClipPlane.Value);

        if (ViewExtensionConfig.MenuOption.Value) SceneManager.activeSceneChanged += CheckSceneState;
    }

    private static void LoadAssetBundle()
    {
        string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string bundlePath = Path.Combine(assemblyDirectory, assetBundleName);

        if (File.Exists(bundlePath))
        {
            assetBundle = AssetBundle.LoadFromFile(bundlePath);
            if (assetBundle != null)
            {
                if (ViewExtensionConfig.DebugLogging.Value) Debug.Log($"[ViewExtension] AssetBundle loaded successfully from path: {bundlePath}");
            }
            else Debug.LogError($"[ViewExtension] Failed to load AssetBundle at path: {bundlePath}");
        }
        else Debug.LogError($"[ViewExtension] AssetBundle not found at path: {bundlePath}");
    }

    private static void CheckSceneState(Scene current, Scene next)
    {
        if (next.name == "SampleSceneRelay")
        {
            if (ViewExtensionConfig.DebugLogging.Value) Debug.Log("[ViewExtension] SampleSceneRelay loaded");
            SpawnPlayerMenu();
            isMainMenu = false;
        }
    }

    private static void SpawnPlayerMenu()
    {
        settingsObject = assetBundle.LoadAsset<GameObject>("ViewDistance");
        if (settingsObject == null)
        {
            Debug.LogError("[ViewExtension] Failed to load ViewDistance from AssetBundle.");
            return;
        }

        GameObject qmManager = GameObject.Find("QuickMenuManager");
        if (qmManager == null)
        {
            Debug.LogError("[ViewExtension] QuickMenuManager object not found.");
            return;
        }

        QuickMenuManager qmManagerScript = qmManager.GetComponent<QuickMenuManager>();
        if (qmManagerScript == null)
        {
            Debug.LogError("[ViewExtension] QuickMenuManager script not found.");
            return;
        }

        GameObject playerMenu = qmManagerScript.settingsPanel?.gameObject;
        if (playerMenu == null)
        {
            Debug.LogError("[ViewExtension] SettingsPanel object not found.");
            return;
        }

        Instantiate(settingsObject, playerMenu.transform, false);
        if (ViewExtensionConfig.DebugLogging.Value) Debug.Log("[ViewExtension] SettingsObject instantiated as child of PlayerMenu.");
    }
}
