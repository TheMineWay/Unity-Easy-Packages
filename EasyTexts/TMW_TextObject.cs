using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TMW_TextObject : MonoBehaviour
{
    private Text UItext;
    public string text {
        set {
            UItext.text = value;
        }
        get {
            return UItext.text;
        }
    }
    public Font font {
        get {
            return UItext.font;
        }
        set {
            UItext.font = value;
        }
    }
    public TextAnchor alignment {
        get {
            return UItext.alignment;
        }
        set {
            UItext.alignment = value;
        }
    }
    public Color color {
        get {
            return UItext.color;
        }
        set {
            UItext.color = value;
        }
    }
    [Header("Easy dialogs")]
    public string dialogID;
    public bool subscribeToChanges = true;
    private void Awake() {
        UItext = GetComponent<Text>();
    }
    void Start()
    {
        GetDialog();
        if(subscribeToChanges) TMW_Config.dialogsDataChanged += GetDialog;
    }

    void GetDialog() {
        if(dialogID != "" && SceneManager.dialogs != null) text = SceneManager.dialogs.GetDialog(dialogID);
    }
}