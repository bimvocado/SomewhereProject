using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("슬라이더 UI")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private Slider autoSpeedSlider;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        textSpeedSlider.onValueChanged.AddListener(OnTextSpeedChanged);
        autoSpeedSlider.onValueChanged.AddListener(OnAutoSpeedChanged);

        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        float savedTypingSpeed = PlayerPrefs.GetFloat("TypingSpeed", 0.05f);
        textSpeedSlider.value = Mathf.InverseLerp(0.1f, 0.01f, savedTypingSpeed);

        float savedAutoDelay = PlayerPrefs.GetFloat("AutoModeDelay", 0.5f);
        autoSpeedSlider.value = Mathf.InverseLerp(0.1f, 1.0f, savedAutoDelay);
    }

    private void OnBGMVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(value);
        }
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }

    private void OnTextSpeedChanged(float value)
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetTypingSpeed(value);
        }
    }

    private void OnAutoSpeedChanged(float value)
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetAutoModeDelay(value);
        }
    }

    private void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        textSpeedSlider.onValueChanged.RemoveListener(OnTextSpeedChanged);
        autoSpeedSlider.onValueChanged.RemoveListener(OnAutoSpeedChanged);
    }
}