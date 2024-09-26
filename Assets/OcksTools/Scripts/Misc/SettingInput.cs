using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInput : MonoBehaviour
{
    public string Type = "";
    public Color32[] color32s = null;
    private Slider slider;
    private Image img;
    bool fard;
    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        img = GetComponent<Image>();
        if (SaveSystem.Instance != null)
            ReadValue();
    }

    public void ToggleVal()
    {
        fard = !fard;
        WriteValue();
        UpdateValue();
    }

    public void WriteValue()
    {
        switch (Type)
        {
            case "MasterVolume":
                SoundSystem.Instance.MasterVolume = slider.value;
                break;
            case "SFXVolume":
                SoundSystem.Instance.SFXVolume = slider.value;
                break;
            case "MusicVolume":
                SoundSystem.Instance.MusicVolume = slider.value;
                break;
            case "ScrollNo":
                SaveSystem.Instance.NoScroll = fard;
                break;
        }
    }

    public void ReadValue()
    {
        switch (Type)
        {
            case "MasterVolume":
                slider.value = SoundSystem.Instance.MasterVolume;
                UpdateValue();
                break;
            case "SFXVolume":
                slider.value = SoundSystem.Instance.SFXVolume;
                UpdateValue();
                break;
            case "MusicVolume":
                slider.value = SoundSystem.Instance.MusicVolume;
                UpdateValue();
                break;
            case "ScrollNo":
                fard = SaveSystem.Instance.NoScroll;
                UpdateValue();
                break;
        }
    }

    public void UpdateValue()
    {
        switch (Type)
        {
            case "ScrollNo":
                img.color = color32s[fard?0:1];
                break;
        }
    }
}
