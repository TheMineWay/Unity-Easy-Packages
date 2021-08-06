using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsController : MonoBehaviour
{
    public static Dictionary<string, int> achievements = new Dictionary<string, int>();
    public readonly string[] achievementIds = {}; // Registry of achievement IDs
    void Start()
    {
        foreach(string key in achievementIds) achievements.Add(key, PlayerPrefs.GetInt(key));
    }

    public void Clear() {
        // Clears all the achievements
        foreach(string key in achievementIds) PlayerPrefs.SetInt(key,0);
    }
}