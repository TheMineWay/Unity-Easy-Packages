using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMW_Logs : MonoBehaviour
{
    public enum ErrorImportance {
        low,
        medium,
        high,
        critical
    }
    public static void LogError(string error, string about = "", ErrorImportance errorImportance = ErrorImportance.low, System.Exception e = null) {
        Debug.LogError((about == "" ? "" : $"[{about}]: ") + error + (e == null ? "" : "\n\tMessage: " + e.Message));
    }
    public static void LogError(string error, string about, System.Exception e) {
        LogError(error, about, ErrorImportance.low, e);
    }
    public static void LogInfo(string message, string about = "") {
        Debug.Log((about == "" ? "" : $"[{about}]: ") + message);
    }
}