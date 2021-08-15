using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasyChatbox : MonoBehaviour
{
    public EasyDialogs_SceneController easyDialogsSceneController;
    public GameObject chatboxObject;
    public TMW_TextObject UI_text, UI_speaker;
    public Chatbox[] chatboxes;
    [Header("Options")]
    public ChatboxAnimation chatboxAnimation;
    [Range(0.0f, 1.0f)]
    public float animationTiming = 0.1f;
    public bool autoControlls = true; // Use Unity Input in order to controll the Chatboxes
    public bool initOnStart = false;

    [Header("Events")]
    public UnityEvent onEnd;

    private Coroutine displayer;
    private int current = 0;

    /* BEHAVIOURS */
    private void Start()
    {
        if (initOnStart) StartCoroutine(WaitUntilLoaded());
        IEnumerator WaitUntilLoaded() {
            yield return new WaitUntil(() => easyDialogsSceneController.Loaded());
            Show(true);
        }
    }

    // New Input System. Uncomment if you are using the new Input System
    /*public void OnNextDialogPressed(UnityEngine.InputSystem.InputAction.CallbackContext button) {
        if(button.performed && autoControlls) NextDialogByUI();
    }*/

    private void Update()
    {
        if (autoControlls)
        {
            // Old input System. Remove if you are using the new Input System
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextDialogByUI();
            }
        }
    }

    private Chatbox chatbox
    {
        get
        {
            return chatboxes[current];
        }
    }
    public void NextDialog(int units = 1)
    { // NOT Controlled by interface (can't be locked)
        SetCurrent(current + units);
    }
    public void SetCurrent(int unit)
    {
        current = unit;
        if (unit < 0) current = 0;
        if (unit >= chatboxes.Length)
        { // End chatbox
            current = chatboxes.Length - 1;
            onEnd.Invoke();
            Hide();
            return;
        }

        EasyDialogs_SceneController.Dialog dialog = easyDialogsSceneController.GetDialogObject(chatbox.dialogID);
        chatbox.onStart.Invoke(); // Trigger on start event

        if (displayer != null) StopCoroutine(displayer);
        displayer = StartCoroutine(Displayer());
        IEnumerator Displayer()
        {
            UI_text.text = "";
            string _dialog = dialog.dialog, _speaker = dialog.speaker;
            if (UI_speaker != null) UI_speaker.text = _speaker;
            string letters = "abcdefghijklmnopqrstuvwxyz1234567890#~€/\\=_";
            switch (chatboxAnimation)
            {
                case ChatboxAnimation.none: UI_text.text = _dialog; break;
                case ChatboxAnimation.type:
                    foreach (char c in _dialog.ToCharArray())
                    {
                        yield return new WaitForSeconds(animationTiming);
                        UI_text.text += c;
                    }
                    break;
                case ChatboxAnimation.fromChangingRandomOrigin:
                    string _toDisplay = "";
                    string rand = "";
                    for (int i = 0; i < _dialog.Length; i++)
                    {
                        yield return new WaitForSeconds(animationTiming);
                        _toDisplay += _dialog[i];
                        rand = "";
                        for (int j = i + 1; j < _dialog.Length; j++) rand += (Random.Range(0, 2) == 0 ? letters[Random.Range(0, letters.Length)].ToString().ToUpper() : letters[Random.Range(0, letters.Length)].ToString());
                        UI_text.text = _toDisplay + rand;
                    }
                    break;
            }
            chatbox.onEnd.Invoke(); // Trigger on end event
            displayer = null;
        }
    }
    public void Hide(bool reset = false)
    { // Hides and disabes the UI
        chatboxObject.SetActive(false);
    }
    public void Show(bool reset = false)
    { // Displays and enables the UI
        chatboxObject.SetActive(true);
        if (reset) current = 0;
        NextDialog(0);
    }
    public void NextDialogByUI()
    {
        if (displayer == null) if (chatbox.locked) return; else NextDialog();
        else
        {
            StopCoroutine(displayer);
            displayer = null;
            UI_text.text = easyDialogsSceneController.GetDialog(chatbox.dialogID);
            chatbox.onEnd.Invoke(); // Trigger on end event
        }
    }

    public enum ChatboxAnimation
    {
        none,
        type,
        fromChangingRandomOrigin
    }

    [System.Serializable]
    public class Chatbox
    {
        public string dialogID;
        public bool displaySpeaker = true, locked = false;
        public UnityEvent onStart, onEnd;
    }
}