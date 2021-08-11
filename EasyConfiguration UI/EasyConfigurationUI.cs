using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EasyConfigurationUI : MonoBehaviour
{
    // VOLUME

    [Header("Volume")]
    public Slider music;
    public Text musicText;
    void SetMusic(float value) {
        if(musicText != null) musicText.text = value.ToString() + "%";
        if(music != null) music.value = value;
    }
    public void OnMusicChange(float value) {
        SetMusic(value);
    }
    public Slider effects;
    public Text effectsText;
    void SetEffects(float value) {
        if(effectsText != null) effectsText.text = value.ToString() + "%";
        if(effects != null) effects.value = value;
    }
    public void OnEffectsChange(float value) {
        SetEffects(value);
    }

    // RESOLUTION

    [Header("Resolution Settings")]
    public int maxHeight = 1080; // Default max height
    [Header("Resolution")]
    public Dropdown resolution;
    public void SetResolution(TMW_Config.Configuration.Resolution res) {
        if(resolution != null) resolution.value = resolution.options.FindIndex((o) => o.text == res.resolution);
        TMW_Kernel.configuration.resolution = res;
    }
    public void OnResolutionChange(int index) {
        if(resolution != null) {
            SetResolution(resolutions[index]);
        }
    }
    // Get resolutions
    public TMW_Config.Configuration.Resolution[] resolutions => Screen.resolutions.Where((res) => res.refreshRate == 60).Where((res) => res.height <= maxHeight).Select((res) => new TMW_Config.Configuration.Resolution(res.width,res.height)).ToArray();
    
    // FPS

    [Header("FPS")]
    public Slider fps;
    public Text fpsText;
    void SetFPS(float value) {
        if(fpsText != null) fpsText.text = value.ToString() + " FPS";
        if(fps != null) fps.value = value;
    }
    public void OnFPSChange(float value) {
        SetFPS(value);
    }

    // ANTI ALIASING
    [Header("Anti Alias")]
    public Dropdown antiAlias;
    public void SetAntiAlias(int value) {
        if(antiAlias != null) antiAlias.value = value;
    }
    public void OnAntiAliasChange(int value) {
        SetAntiAlias(value);
    }

    // LANGUAGE SELECTOR
    [Header("Language")]
    public Dropdown languageSelector;
    public string[] languages;

    void SetLanguage(TMW_Config.Language language) {
        if(languageSelector != null) {
            languageSelector.value = (int)language;
            if(languages.Length > (int)language && (int)language >= 0) languageSelector.captionText.text = languages[(int)language];
        }
    }

    public void OnLanguageChange(int value) {
        SetLanguage((TMW_Config.Language)value);
    }

    // GENDER SELECTOR
    [Header("Referal gender")]
    public Dropdown referalGenderSelector;
    public string[] genderIds;

    void SetReferalGender(TMW_Config.ReferalGender referalGender) {
        if(referalGenderSelector != null) {
            referalGenderSelector.value = (int)referalGender;
            if(genderIds.Length > (int)referalGender && (int)referalGender >= 0) referalGenderSelector.captionText.text = SceneManager.dialogs.GetDialog(genderIds[(int)referalGender]);
        }
    }

    public void OnReferalGenderChange(int value) {
        SetReferalGender((TMW_Config.ReferalGender)value);
    }

    // END

    public void Load() {
        if(TMW_Kernel.configuration == null) {
            TMW_Kernel.configuration = new TMW_Config.Configuration();
            TMW_Kernel.configuration.resolution = new TMW_Config.Configuration.Resolution(Screen.width, Screen.height);
        }
        TMW_Config.Configuration conf = TMW_Kernel.configuration;
        SetMusic(conf.music);
        SetEffects(conf.effects);
        if(resolution != null) {
            resolution.options.Clear();
            resolution.options.AddRange(resolutions.Select((el) => new Dropdown.OptionData(el.resolution)));
        }
        if(antiAlias != null) {
            antiAlias.options.Clear();
            antiAlias.options.Add(new Dropdown.OptionData("Disabled"));
            for(int i = 2; i <= conf.maxAntialiasingLevel; i*=2) antiAlias.options.Add(new Dropdown.OptionData(i.ToString() + " Multi Sampling"));
        }
        SetAntiAlias(conf.antiAliasing / 2);
        SetResolution(conf.resolution);
        SetFPS(conf.fps);
        if(languageSelector != null) {
            languageSelector.options.Clear();
            foreach(string lang in languages) languageSelector.options.Add(new Dropdown.OptionData(lang));
        }
        SetLanguage(conf.language);
        if(referalGenderSelector != null) {
            referalGenderSelector.options.Clear();
            foreach(string dialogID in genderIds) referalGenderSelector.options.Add(new Dropdown.OptionData(SceneManager.dialogs.GetDialog(dialogID)));
        }
        SetReferalGender(conf.referalGender);
    }

    public void Save() {
        if (TMW_Kernel.configuration == null) Debug.LogError("NULL CONF");
        if (music != null) TMW_Kernel.configuration.music = music.value;
        if (effects != null) TMW_Kernel.configuration.effects = effects.value;
        if (resolution != null) TMW_Kernel.configuration.resolution = resolutions[resolution.value];
        if (fps != null) TMW_Kernel.configuration.fps = (int)fps.value;
        if (antiAlias != null) TMW_Kernel.configuration.antiAliasing = (int)antiAlias.value * 2;
        if (languageSelector != null) TMW_Kernel.configuration.language = (TMW_Config.Language)languageSelector.value;
        if (referalGenderSelector != null) TMW_Kernel.configuration.referalGender = (TMW_Config.ReferalGender)referalGenderSelector.value;
        if (languageSelector != null || referalGenderSelector != null) {
            SceneManager.dialogs.LoadDialogs();
            TMW_Config.dialogsDataChanged(); // Call the delegate in order to modify the existing dialogs
        }
        TMW_Kernel.configuration.Save();
        Load(); // Reloads UI
    }
    void Start()
    {
        Load();
    }
}