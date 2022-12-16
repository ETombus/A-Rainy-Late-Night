using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [Header("Mixers")]
    [SerializeField] AudioMixer mixer;

    [Header("Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    [Header("Toggles")]
    [SerializeField] Toggle muteMusicToggle;
    [SerializeField] Toggle muteSoundToggle;

    [Header("SoundTest")]
    [SerializeField] AudioClip[] clips;
    AudioSource audSource;

    private void Start()
    {
        audSource = GetComponent<AudioSource>();

        SetSliderValues();

        if (PlayerPrefs.GetFloat("MusicMuted") == 0) { muteMusicToggle.isOn = false; }
        else
        {
            muteMusicToggle.isOn = true;
            MuteMusic(true);
        }

        if (PlayerPrefs.GetFloat("SoundMuted") == 0) { muteSoundToggle.isOn = false; }
        else
        {
            muteSoundToggle.isOn = true;
            MuteSound(true);
        }
    }

    void SetSliderValues()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        //Debug.Log("sliderValue = " + sliderValue);
    }

    public void SetSoundVolume(float sliderValue)
    {
        mixer.SetFloat("SoundVol", ConvertLog(sliderValue));
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }

    public void MuteMusic(bool isMuted)
    {
        if (isMuted)
        {
            //Debug.Log("Muted Music");
            musicSlider.interactable = false;
            mixer.SetFloat("MusicVol", -80);
            PlayerPrefs.SetFloat("MusicMuted", 1);
        }
        else
        {
            //Debug.Log("Unmuted Music");
            musicSlider.interactable = true;
            mixer.SetFloat("MusicVol", ConvertLog(musicSlider.value));
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
            //Debug.Log("Music Slider value is: " + musicSlider.value);
            PlayerPrefs.SetFloat("MusicMuted", 0);
        }
    }

    public void MuteSound(bool isMuted)
    {
        if (isMuted)
        {
            soundSlider.interactable = false;
            mixer.SetFloat("SoundVol", -80);
            PlayerPrefs.SetFloat("SoundMuted", 1);
        }
        else
        {
            soundSlider.interactable = true;
            mixer.SetFloat("SoundVol", ConvertLog(soundSlider.value));
            PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
            PlayerPrefs.SetFloat("SoundMuted", 0);
        }
    }

    float ConvertLog(float value)
    {
        float convertedValue = Mathf.Log10(value) * 20;
        return convertedValue;
    }

    public void SoundTest()
    {
        audSource.pitch = Random.Range(0.8f, 1.2f);
        audSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
