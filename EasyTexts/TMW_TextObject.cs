using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMW_TextObject : MonoBehaviour
{
    private TextMeshProUGUI UItext;
    public string text {
        set {
            UItext.text = value;
        }
        get {
            return UItext.text;
        }
    }
    [Header("Easy dialogs")]
    public string dialogID;
    public bool subscribeToChanges = true;
    private void Awake() {
        UItext = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        StartCoroutine(WaitUntilDialogsLoaded());
        IEnumerator WaitUntilDialogsLoaded() {
            yield return new WaitUntil(SceneManager.dialogs.Loaded);
            GetDialog();
            if(subscribeToChanges) TMW_Config.dialogsDataChanged += GetDialog;   
        }
    }

    void GetDialog() {
        if(dialogID != "" && SceneManager.dialogs != null) text = SceneManager.dialogs.GetDialog(dialogID);
    }
}