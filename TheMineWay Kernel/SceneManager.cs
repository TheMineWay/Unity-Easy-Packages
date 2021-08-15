using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static EasyDialogs_SceneController dialogs;
    public EasyDialogs_SceneController dialogsController;

    private void Awake() {
        if(dialogsController != null) {
            dialogs = null;
            dialogs = dialogsController;
        }

        // Init configuration controller
        TMW_Config.LoadConfig();
    }

    private void Update() {
        // Old Input System. Remove if you are using the new Input System
        if(Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
    }
}