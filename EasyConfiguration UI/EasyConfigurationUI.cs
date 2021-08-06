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

    // END

    public void Load() {
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
    }

    public void Save() {
        if(music != null) TMW_Kernel.configuration.music = music.value;
        if(effects != null) TMW_Kernel.configuration.effects = effects.value;
        if(resolution != null) TMW_Kernel.configuration.resolution = resolutions[resolution.value];
        if (fps != null) TMW_Kernel.configuration.fps = (int)fps.value;
        if (antiAlias != null) TMW_Kernel.configuration.antiAliasing = (int)antiAlias.value * 2;
        TMW_Kernel.configuration.Save();
        Load(); // Reloads UI
    }
    void Start()
    {
        Load();
    }
}