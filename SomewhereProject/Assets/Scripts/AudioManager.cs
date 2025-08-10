using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("¿Àµð¿À ¹Í¼­")]
    [SerializeField] private AudioMixer masterMixer;


    private void Start()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SetBGMVolume(bgmVolume);

        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SetSFXVolume(sfxVolume);
    }

    public void SetBGMVolume(float volume)
    {
        masterMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}