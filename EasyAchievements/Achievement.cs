using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Achievement : MonoBehaviour
{
    private RectTransform rectTransform;
    public string id;
    public new string name;
    public int value = 0;
    public int max = 100; // Amount of steps to reach this achievement
    [Header("Objects")]
    public GameObject progressBar;
    public GameObject completedObject;
    public Text nameText;
    public Text valueText;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        try {
            value = AchievementsController.achievements[id];
        } catch(System.Exception e) {
            value = 0;
        }
        if(value > max) {
            value = max;
            AchievementsController.achievements[id] = max;
        }
        DisplayBar();
    }

    void DisplayBar() {
        if(valueText != null) valueText.text = value.ToString() + "/" + max.ToString();
        if(nameText != null) nameText.text = name;
        if(completedObject != null) completedObject.SetActive(value >= max);

        float width = rectTransform.rect.width;
        float percent = ((float)value / (float)max) * 100;
        RectTransform rt = progressBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2((((width * percent) / 100) - width) * 2, rt.sizeDelta.y);
        rt.offsetMin = Vector3.zero;
    }
}