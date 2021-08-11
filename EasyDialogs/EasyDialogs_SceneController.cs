using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EasyDialogs_SceneController : MonoBehaviour
{
    private static TMW_Config.Language language
    {
        get
        {
            return TMW_Kernel.configuration.language;
        }
    }
    private static TMW_Config.ReferalGender gender
    {
        get
        {
            return TMW_Kernel.configuration.referalGender;
        }
    }

    public TextAsset[] dialogFiles;
    private bool loaded = false; // Are the dialogs fully loaded?

    public Dictionary<string, Dialog> dialogs = new Dictionary<string, Dialog>(); // Dialogs storage
    public bool loadOnAwake; // Autoload dialogs (recommended)
    public bool missResistance; // Load a base language in order to controll missing dialogs (recommended if there is more than one language)

    /* UNITY FUNCTIONS */
    private void Start()
    {
        if(TMW_Kernel.configuration == null) {
            TMW_Kernel.configuration = new TMW_Config.Configuration();
            TMW_Kernel.configuration.language = TMW_Config.Language.english;
            TMW_Kernel.configuration.referalGender = TMW_Config.ReferalGender.inclusive;
            TMW_Logs.LogError("Configuration not loaded! Applying default values","Configuration");
        }
        dialogs.Clear();
        if (loadOnAwake) LoadDialogs();
    }

    /* BEHAVIOURS */
    public void LoadDialogs()
    {
        loaded = false;
        dialogs.Clear();
        if (missResistance && dialogFiles.Length >= 2) LoadDialog(0, false);
        LoadDialog(language, false);
        loaded = true;
        TMW_Logs.LogInfo("Dialogs loaded", "Dialogs");
    }
    private void LoadDialog(TMW_Config.Language language, bool clear = false)
    {
        DialogFile dialogFile = JsonConvert.DeserializeObject<DialogFile>(dialogFiles[(int)language].text);
        Dictionary<string, Dialog> virtualDialogs = dialogFile.dialogs;
        if (clear) dialogs.Clear();
        // Load gender variables
        Dictionary<string, string[]> genderVariables = dialogFile.genderVariables;

        // Begin memory storage
        foreach (string key in virtualDialogs.Keys)
        {
            Dialog virt = virtualDialogs[key];
            string _dialog = virt.GetDialog();
            if (virt.usesGenderVariables)
            {
                foreach (string _key in genderVariables.Keys)
                {
                    if (!_dialog.Contains(virt.genderChar.ToString())) break;
                    _dialog = _dialog.Replace(virt.genderChar + _key + virt.genderChar, genderVariables[_key][(int)gender]);
                }
            }
            if (dialogs.ContainsKey(key)) dialogs[key] = new Dialog(_dialog, virt.speaker, virt.usesGenderVariables, virt.genderChar);
            else dialogs.Add(key, new Dialog(_dialog, virt.speaker, virt.usesGenderVariables, virt.genderChar));
        }
    }
    public Dialog GetDialogObject(string key)
    {
        if (dialogs.ContainsKey(key)) return dialogs[key];
        TMW_Logs.LogError($"Cannot find the dialog \"{key}\". An empty dialog has been displayed.", "Dialog error", TMW_Logs.ErrorImportance.critical);
        return Dialog.empty;
    }
    public string GetDialog(string _key)
    {
        return GetDialogObject(_key).GetDialog();
    }
    public string GetSpeaker(string key)
    {
        return GetDialogObject(key).speaker;
    }

    public bool Loaded() {
        return loaded;
    }

    public class DialogFile
    {
        public int version;
        public string name, by;
        public System.DateTime lastUpdate;
        public Dictionary<string, Dialog> dialogs; // Dialogs storage
        public Dictionary<string, string[]> genderVariables; // Gender variables storage
    }
    public class Dialog
    {
        public string dialog, speaker;
        public readonly char genderChar = '#';
        public readonly bool usesGenderVariables = false; // False by default

        public string GetDialog()
        {
            return dialog;
        }
        public Dialog(string dialog = "", string speaker = "", bool usesGenderVariables = false, char genderChar = '#')
        {
            this.dialog = dialog;
            this.speaker = speaker;
            this.usesGenderVariables = usesGenderVariables;
            this.genderChar = genderChar;
        }

        public static Dialog empty
        {
            get
            {
                return new Dialog();
            }
        }
    }
}