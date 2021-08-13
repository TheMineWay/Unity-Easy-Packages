using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TMW_Config
{
    public enum Language
    {
        english,
        spanish
    }
    public enum ReferalGender
    {
        female,
        male,
        inclusive
    }

    public static void LoadConfig()
    {
        try
        {
            string configData = TMW_Kernel.configData;
            TMW_Kernel.configuration = JsonConvert.DeserializeObject<TMW_Config.Configuration>(configData); // Read and load config
            TMW_Logs.LogInfo("Applied configuration", "Configuration");
        }
        catch (System.Exception e)
        {
            TMW_Logs.LogError($"Cannot read the configuration. Applying default configuration.\n\tThe error will persist until the configuration is rewritten.", "Configuration", e);
            TMW_Kernel.configuration = Configuration.empty;
            TMW_Kernel.configuration.Save();
        }
    }

    public delegate void EffectsChangedDelegate();
    public static EffectsChangedDelegate effectsChanged;
    public delegate void MusicChangedDelegate();
    public static MusicChangedDelegate musicChanged;
    public delegate void DialogsDataChanged();
    public static DialogsDataChanged dialogsDataChanged;

    // END LISTENERS
    public class Configuration
    {

        Language _language;
        public Language language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }
        ReferalGender _referalGender;
        public ReferalGender referalGender {
            get {
                return _referalGender;
            }
            set {
                _referalGender = value;
            }
        }

        /* Constructors */
        public Configuration()
        {
            // Write here default configuration
            language = default(Language);
            referalGender = default(ReferalGender);
            resolution = Resolution.DefaultResolution();
            music = 75;
            effects = 75;
            vSync = true;
            fps = 60;
        }

        // *************************************************************
        // RESOLUTION
        // *************************************************************

        public Resolution resolution;

        public class Resolution
        {
            public int width;
            public int height;
            public string resolution
            {
                get
                {
                    return $"{width}x{height}";
                }
            }
            public Resolution(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
            public static Resolution DefaultResolution()
            {
                return new Resolution(Screen.width, Screen.height);
            }
        }

        // *************************************************************
        // FPS
        // *************************************************************
        public int _fps;
        public int fps
        {
            get
            {
                return _fps == 0 ? 60 : _fps;
            }
            set
            {
                if (value >= 30) _fps = value;
                else _fps = 60;
            }
        }

        // *************************************************************
        // VSYNC
        // *************************************************************
        public bool vSync;

        // *************************************************************
        // VOLUME
        // *************************************************************

        private float _music;
        public float music
        {
            get { return _music; }
            set
            {
                if (music >= 0 && music <= 100) _music = value;
                else _music = 75;
            }
        }
        private float _effects;
        public float effects
        {
            get { return _effects; }
            set
            {
                if (effects >= 0 && effects <= 100) _effects = value;
                else _effects = 75;
            }
        }

        // *************************************************************
        // ANTI ALIASING
        // *************************************************************

        [Header("Anti Aliasing Configuration")]
        public readonly int maxAntialiasingLevel = 8; // Default anti aliasing level
        [Header("Anti Aliasing")]
        private int _antiAliasing;
        public int antiAliasing
        {
            get { return _antiAliasing; }
            set
            {
                if (value >= 0 && value <= maxAntialiasingLevel) _antiAliasing = value;
                else _antiAliasing = 0;
            }
        }


        // *************************************************************
        // END
        // *************************************************************
        public static Configuration empty
        {
            get
            {
                return new Configuration();
            }
        }

        // Save the configuration
        public void Save()
        {
            string configData = JsonConvert.SerializeObject(this);
            TMW_Kernel.configData = configData;
            if (musicChanged != null) musicChanged();
            if (effectsChanged != null) effectsChanged();
            if (resolution != null) Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            Application.targetFrameRate = fps;
            QualitySettings.antiAliasing = antiAliasing;
        }
        public void Load()
        {
            string configData = TMW_Kernel.configData;
            TMW_Kernel.configuration = JsonConvert.DeserializeObject<Configuration>(configData);
        }
    }
}