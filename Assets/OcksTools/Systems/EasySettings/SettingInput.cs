using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInput : MonoBehaviour
{
    public string Type = "";
    public string Type2 = "";
    public Color32[] color32s = null;
    private Slider slider;
    private Image img;
    private Switcher sw;
    private KeybindInput ky;
    [HideInInspector]
    public bool fard;
    bool hasattached = false;

    public System.Action UpdateSex = null;
    bool sharted = false;
    private void OnEnable()
    {
        if (Time.time < 0.1f) return;
        slider = GetComponent<Slider>();
        img = GetComponent<Image>();
        sw = GetComponent<Switcher>();
        ky = GetComponent<KeybindInput>();
        if (!hasattached)
        {
            hasattached = true;
            SaveSystem.LoadAllData += ReadValue;
        }

        if (!sharted)
        {
            sharted = true;
            switch (Type)
            {
                case "KeyRelay":
                    UpdateSex += ky.upup;
                    break;
            }
        }

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
            case "TestToggle":
                SaveSystem.Instance.TestBool = fard;
                break;
            case "TestSwitcher":
                SaveSystem.Instance.test = sw.index;
                break;
            case "TestKeybind":
                SaveSystem.Instance.testkeybind = ky.keyCode;
                break;
            case "KeyRelay":
                InputManager.gamekeys[Type2] = new List<KeyCode>() { ky.keyCode };
                break;
            case "Highlights":
                Gamer.Instance.Highlights = slider.value / 20f;
                Gamer.Instance.UpdateShaders();
                break;
            case "Lowlights":
                Gamer.Instance.Lowlights = slider.value / 20f;
                Gamer.Instance.UpdateShaders();
                break;
            case "ScrollNo":
                SaveSystem.Instance.NoScroll = fard;
                break;
        }
        UpdateSex?.Invoke();
    }
    
    public void ReadValue()
    {
        string dict = "def";
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
            case "TestToggle":
                fard = SaveSystem.Instance.TestBool;
                UpdateValue();
                break;
            case "TestSwitcher":
                sw.index = SaveSystem.Instance.test;
                UpdateValue();
                break;
            case "TestKeybind":
                ky.keyCode = SaveSystem.Instance.testkeybind;
                UpdateValue();
                break;
            case "KeyRelay":
                if (Type2 == "") return;
                ky.keyCode = InputManager.gamekeys[Type2][0];
                UpdateValue();
                break;
            case "Highlights":
                slider.value = Gamer.Instance.Highlights * 20;
                UpdateValue();
                break;
            case "Lowlights":
                slider.value = Gamer.Instance.Lowlights * 20;
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
            case "TestToggle":
            case "ScrollNo":
                img.color = color32s[fard ? 0 : 1];
                break;
        }
        UpdateSex?.Invoke();
    }
}
