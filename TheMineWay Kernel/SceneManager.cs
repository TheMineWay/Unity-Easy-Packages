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
        //TMW_Kernel.configData = "ss"; // Throws error
        TMW_Config.LoadConfig();
        //TMW_Logs.LogInfo(TMW_Kernel.configuration.resolution.width.ToString());
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
    }
}