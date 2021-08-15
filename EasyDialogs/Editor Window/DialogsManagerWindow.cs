using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using System.IO;

public class DialogsManagerWindow : EditorWindow
{
    [MenuItem("Window/Dialogs manager")]
    public static void ShowWindow()
    {
        GetWindow<DialogsManagerWindow>("Dialogs manager");
    }
    private FileInfo dialogFileInfo;

    string idFilter = "", contentFilter = "", usingGenderVariable = "";
    string gvNameFilter = "";

    // Virtual Easy Dialogs BBDD
    private EasyDialogs_SceneController.DialogFile dialogFile;
    private DialogContainer[] dialogs => dialogFile.dialogs.Where((d) => d.Key.ToLower().Contains(idFilter.ToLower()) || idFilter == "").Where((d) => d.Value.dialog.ToLower().Contains(contentFilter.ToLower()) || contentFilter == "").Where((d) => d.Value.dialog.Contains($"{d.Value.genderChar}{usingGenderVariable}{d.Value.genderChar}") || usingGenderVariable == "").Select((d) => new DialogContainer(d.Key, d.Value)).ToArray();
    private bool viewDialogsOptions = true, viewGenderVariablesOptions = false;
    DialogContainer selectedContainer;
    GenderVariablesContainer selectedGenderVariablesContainer;
    GenderVariablesContainer[] genderVariables => dialogFile.genderVariables.Where((v) => v.Key.ToLower().Contains(gvNameFilter.ToLower()) || gvNameFilter == "").Select((v) => new GenderVariablesContainer(v.Key, dialogFile)).ToArray();
    private void OnGUI()
    {
        var dangerBtn = new GUIStyle(GUI.skin.button);
        if (dialogFileInfo != null) dangerBtn.normal.textColor = new Color(240f / 255f, 100f / 255f, 100f / 255f);
        GUILayout.Label("The official Easy Dialogs manager.");
        if (GUILayout.Button($"Load dialog file{(dialogFileInfo == null ? "" : " (will override changes)")}", dangerBtn)) LoadDialogFile();
        if (dialogFileInfo != null)
        {
            // There is a dialog!
            viewDialogsOptions = EditorGUILayout.Toggle("View dialog options", viewDialogsOptions);
            if (viewDialogsOptions)
            {
                GUILayout.Label("Dialog file info", EditorStyles.boldLabel);
                dialogFile.by = EditorGUILayout.TextField("Author", dialogFile.by);
                dialogFile.name = EditorGUILayout.TextField("Name (language)", dialogFile.name);
                dialogFile.version = EditorGUILayout.IntField("Version", dialogFile.version);
                dialogFile.lastUpdate = System.DateTime.Parse(EditorGUILayout.TextField("Last update (dd-mm-yyyy)", dialogFile.lastUpdate.ToString("dd-MM-yyyy")));
                if (GUILayout.Button("Set to now")) dialogFile.lastUpdate = System.DateTime.Now;
                if (selectedContainer != null)
                {
                    // There is a dialog container
                    Separator();
                    GUILayout.Label("Dialog editor", EditorStyles.boldLabel);
                    string newId = EditorGUILayout.TextField("Dialog ID", selectedContainer.id);
                    if (selectedContainer.id != newId)
                    {
                        if (dialogFile.dialogs.Keys.Contains(newId))
                        {
                            Debug.LogError($"There is already a dialog using the \"{newId}\" id!");
                            return;
                        }
                        dialogFile.dialogs.Remove(selectedContainer.id);
                        dialogFile.dialogs.Add(newId, selectedContainer.dialog);
                        selectedContainer.id = newId;
                    }
                    selectedContainer.dialog.dialog = EditorGUILayout.TextField("Dialog", selectedContainer.dialog.dialog);
                    selectedContainer.dialog.speaker = EditorGUILayout.TextField("Speaker", selectedContainer.dialog.speaker);
                    selectedContainer.dialog.usesGenderVariables = EditorGUILayout.Toggle("Use gender variables?", selectedContainer.dialog.usesGenderVariables);
                    if (selectedContainer.dialog.usesGenderVariables)
                    {
                        string genderChar = EditorGUILayout.TextField("Gender char", selectedContainer.dialog.genderChar.ToString());
                        selectedContainer.dialog.genderChar = genderChar.Length > 0 ? genderChar[0] : ' ';
                    }
                    if (GUILayout.Button("[-] Delete dialog"))
                    {
                        dialogFile.dialogs.Remove(selectedContainer.id);
                        selectedContainer = null;
                    }
                }
                Separator();
                GUILayout.Label("Dialog selector", EditorStyles.boldLabel);
                idFilter = EditorGUILayout.TextField("Filter by id", idFilter);
                contentFilter = EditorGUILayout.TextField("Filter by content", contentFilter);
                usingGenderVariable = EditorGUILayout.TextField("Filter by gender variable", usingGenderVariable);
                DisplayDialogs();
                Separator();
                GUILayout.Label("Dialog actions", EditorStyles.boldLabel);
                if (GUILayout.Button("[+] Add dialog"))
                {
                    string newId = "new dialog id";
                    int c = 1;
                    EasyDialogs_SceneController.Dialog dialog = new EasyDialogs_SceneController.Dialog();
                    while (dialogFile.dialogs.Keys.Contains(newId)) newId = $"new dialog ({c++})";
                    DialogContainer dc = new DialogContainer(newId, dialog);
                    dialogFile.dialogs.Add(newId, dialog);
                }
                if (GUILayout.Button("Unselect")) selectedContainer = null;
                Separator();
            }
            viewGenderVariablesOptions = EditorGUILayout.Toggle("View gender variables options", viewGenderVariablesOptions);
            if (viewGenderVariablesOptions)
            {
                int totalGenders = EditorGUILayout.IntField("Total genders", dialogFile.totalGenders);
                dialogFile.totalGenders = totalGenders >= 0 ? totalGenders : 0;
                if (selectedGenderVariablesContainer != null)
                {
                    Separator();
                    GUILayout.Label("Gender variable editor", EditorStyles.boldLabel);
                    string newName = EditorGUILayout.TextField("Variable name", selectedGenderVariablesContainer.variableName);
                    if (newName != selectedGenderVariablesContainer.variableName)
                    {
                        // Name changed
                        if (dialogFile.genderVariables.Keys.Contains(newName))
                        {
                            Debug.LogError($"There is already a gender variable using the \"{newName}\" variable name!");
                            return;
                        }
                        dialogFile.genderVariables.Remove(selectedGenderVariablesContainer.variableName);
                        dialogFile.genderVariables.Add(newName, selectedGenderVariablesContainer.values);
                    }
                    GUILayout.Space(4);
                    ResizeGenderVariables();
                    for (int i = 0; i < dialogFile.totalGenders; i++) selectedGenderVariablesContainer.values[i] = EditorGUILayout.TextField($"({(i + 1)}) " + ((TMW_Config.ReferalGender)i).ToString(), selectedGenderVariablesContainer.values[i]);
                    GUILayout.Space(4);
                    if(GUILayout.Button("Select dialogs using this variable (filter)")) usingGenderVariable = selectedGenderVariablesContainer.variableName;
                    if(GUILayout.Button("[-] Remove gender variable")) {
                        dialogFile.genderVariables.Remove(selectedGenderVariablesContainer.variableName);
                        selectedGenderVariablesContainer = null;
                    }
                    Separator();
                }
                GUILayout.Label("Gender variable selector", EditorStyles.boldLabel);
                gvNameFilter = EditorGUILayout.TextField("Filter by name", gvNameFilter);
                DisplayGenderVariables();
                Separator();
                GUILayout.Label("Gender variable actions", EditorStyles.boldLabel);
                if(GUILayout.Button("[+] Add gender variable")) {
                    string newName = "new gender variable name";
                    int c = 1;
                    while (dialogFile.genderVariables.Keys.Contains(newName)) newName = $"new gender variable name ({c++})";
                    dialogFile.genderVariables.Add(newName, new string[dialogFile.totalGenders]);
                }
                if(GUILayout.Button("Clean gender variables")) CleanGenderVariables();
            }
            Separator();
            if (GUILayout.Button($"Save changes onto \"{dialogFileInfo.Name}\"")) SaveDialogFile();
        }
    }
    private void Separator(int i_height = 1, bool space = true)
    {
        if (space) GUILayout.Space(5);
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        if (space) GUILayout.Space(5);
    }
    private void DisplayDialogs()
    {
        foreach (DialogContainer dc in dialogs) if (GUILayout.Button(dc.id)) selectedContainer = dc;
    }
    private void DisplayGenderVariables()
    {
        foreach (GenderVariablesContainer gvc in genderVariables)
        {
            if (GUILayout.Button(gvc.variableName)) selectedGenderVariablesContainer = gvc;
        }
    }
    private void ResizeGenderVariables()
    {
        foreach (GenderVariablesContainer gvc in genderVariables) {
            List<string> resizeObj = new List<string>();
            resizeObj.AddRange(gvc.values);
            while (gvc.values.Length < dialogFile.totalGenders)
            {
                // Resize array
                resizeObj.Add("");
                gvc.values = resizeObj.ToArray();
            }
        }
    }
    private void CleanGenderVariables() {
        string[] keys = dialogFile.genderVariables.Keys.ToArray();
        foreach(string key in keys) {
            string[] vals = new string[dialogFile.totalGenders];
            for(int i = 0; i < vals.Length && i < dialogFile.genderVariables[key].Length; i++) vals[i] = dialogFile.genderVariables[key][i];
            dialogFile.genderVariables[key] = vals;
        }
    }
    private void LoadDialogFile()
    {
        string filePath = EditorUtility.OpenFilePanel("Select dialog file", Directory.GetCurrentDirectory(), "*");
        if (filePath == "") return;
        try
        {
            dialogFileInfo = new FileInfo(filePath);
            FileStream readStream = dialogFileInfo.OpenRead();
            string content = new StreamReader(readStream).ReadToEnd();
            readStream.Close();

            dialogFile = JsonConvert.DeserializeObject<EasyDialogs_SceneController.DialogFile>(content);
            viewDialogsOptions = false;
            viewGenderVariablesOptions = false;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Something went wrong while parsing the file. Message: " + e.Message);
            dialogFileInfo = null;
            dialogFile = null;
            selectedContainer = null;
        }
    }
    private void SaveDialogFile()
    {
        try
        {
            File.WriteAllText(dialogFileInfo.FullName, JsonConvert.SerializeObject(dialogFile));
        }
        catch (System.Exception e)
        {
            Debug.LogError("Something went wrong while writing the file. Message: " + e.Message);
        }
    }

    [System.Serializable]
    public class DialogContainer
    {
        public string id;
        [SerializeField] public EasyDialogs_SceneController.Dialog dialog;

        public DialogContainer(string id, EasyDialogs_SceneController.Dialog dialog)
        {
            this.dialog = dialog;
            this.id = id;
        }
    }
    public class GenderVariablesContainer
    {
        public string variableName;
        private EasyDialogs_SceneController.DialogFile dialogFile;
        public string[] values
        {
            get
            {
                return dialogFile.genderVariables[variableName];
            }
            set
            {
                dialogFile.genderVariables[variableName] = value;
            }
        }

        public GenderVariablesContainer(string variableName, EasyDialogs_SceneController.DialogFile dialogFile)
        {
            this.variableName = variableName;
            this.dialogFile = dialogFile;
        }
    }
}