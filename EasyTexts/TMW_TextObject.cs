using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TMW_TextObject : MonoBehaviour
{
    private Text UItext;

    [Header("General text options")]
    [TextArea]
    public string initialText;
    public bool customConfig = false;
    public Font initialFont;
    public TextAnchor initialAlignment;
    public Color initialColor = Color.black;
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
    private void Awake() {
        UItext = GetComponent<Text>();
        if(!customConfig) return;
        text = initialText;
        if(font != null) font = initialFont;
        alignment = initialAlignment;
        color = initialColor;
    }
    void Start()
    {
        if(dialogID != "" && SceneManager.dialogs != null) text = SceneManager.dialogs.GetDialog(dialogID);
    }
}