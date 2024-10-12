using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BepInEx;
using GameNetcodeStuff;

public class ViewExtensionSettings : MonoBehaviour
{
    public TextMeshProUGUI sliderText;
    private Slider slider;
    private Camera gameplayCamera;
    private Camera spectateCamera;

    void Awake()
    {
        slider = GetComponent<Slider>();
        if (sliderText == null) { Debug.LogError("[ViewExtension] Slider text reference is missing."); return; }

        slider.onValueChanged.AddListener(OnSettingsChange);

        // Initialize slider value and text
        float initialValue = ViewExtensionConfig.FarClipPlane.Value / 50f;
        slider.value = initialValue;
        UpdateSliderText(ViewExtensionConfig.FarClipPlane.Value);

        if (!ViewExtensionLoader.isMainMenu)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                PlayerControllerB playerController = player.GetComponent<PlayerControllerB>();
                if (playerController != null)
                {
                    gameplayCamera = playerController.gameplayCamera;
                    gameplayCamera.farClipPlane = ViewExtensionConfig.FarClipPlane.Value;
                }
                else Debug.LogError("[ViewExtension] PlayerControllerB script not found on Player object.");
            }
            else Debug.LogError("[ViewExtension] Player object not found.");
            
            if (StartOfRound.Instance) {
                spectateCamera = StartOfRound.Instance.spectateCamera;
            }
            else Debug.LogError("[ViewExtension] StartOfRound object not found.");
        }
    }

    public void OnSettingsChange(float tempDistanceValue)
    {
        int newDistanceValue = Mathf.RoundToInt(tempDistanceValue * 50);
        ViewExtensionConfig.FarClipPlane.Value = newDistanceValue;

        UpdateSliderText(newDistanceValue);

        if (ViewExtensionConfig.DebugLogging.Value) Debug.Log(sliderText.text);

        if (gameplayCamera != null) gameplayCamera.farClipPlane = newDistanceValue;
        if (spectateCamera != null) spectateCamera.farClipPlane = newDistanceValue;

        ViewExtensionConfig.Save();
    }

    private void UpdateSliderText(float value)
    {
        sliderText.text = "View Distance: " + Mathf.RoundToInt(value).ToString("G");
    }
}
