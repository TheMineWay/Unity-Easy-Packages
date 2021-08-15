using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public class DialogCreatorWindow : EditorWindow
{
    [MenuItem("Window/Easy Dialogs/Dialog creator")]
    public static void ShowWindow()
    {
        GetWindow<DialogCreatorWindow>("Dialog creator");
    }

    TMW_Config.Language language = default(TMW_Config.Language);
    string name = default(TMW_Config.Language).ToString();

    private void OnGUI() {
        EditorGUILayout.LabelField("This window can be used to create dialog files using a easy UI.");
        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("New file info", EditorStyles.boldLabel);
        language = (TMW_Config.Language)EditorGUILayout.EnumPopup(language);
        name = EditorGUILayout.TextField("Asset name (ex: english)", name);

        if(GUILayout.Button("Create Easy Dialogs asset")) CreateFile();
        EditorGUILayout.Space(10);
        if(EditorGUILayout.LinkButton("GitHub repository")) Application.OpenURL("https://github.com/TheMineWay/Unity-Easy-Packages");
    }

    private void CreateFile() {
        string path = EditorUtility.SaveFilePanel("Create new Easy Dialogs asset", Directory.GetCurrentDirectory(),$"new {language.ToString().ToLower()} dialog","json");
        Debug.Log(path);
        EasyDialogs_SceneController.DialogFile dialogFile = new EasyDialogs_SceneController.DialogFile();
        dialogFile.lastUpdate = System.DateTime.Now;
        dialogFile.version = 1;
        dialogFile.name = name;
        dialogFile.genderVariables = new Dictionary<string, string[]>();
        dialogFile.dialogs = new Dictionary<string, EasyDialogs_SceneController.Dialog>();
        File.WriteAllText(path, JsonConvert.SerializeObject(dialogFile));
    }
}