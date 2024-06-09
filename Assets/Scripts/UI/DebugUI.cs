using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DebugUI : MonoBehaviour
{
    public TimeManager timeManager;
    public TextMeshProUGUI dayText;
    public GameObject dayUI;
    public GameObject varUI;
    public bool dayIsVisible;
    public bool varIsVisible;
    // Start is called before the first frame update
    void Start()
    {
        ToggleDay(dayIsVisible);
        ToggleVar(varIsVisible);
        if (timeManager == null)
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.debugMode)
            return;
        Keyboard k = Keyboard.current;
        if (k.rightBracketKey.wasPressedThisFrame)
        {
            ToggleDay();
        }
        if (dayIsVisible)
        {
            UpdateDay();
        }

        if (k.leftBracketKey.wasPressedThisFrame) {
            ToggleVar();
        }
    }

    void ToggleDay(bool setting)
    {
        dayIsVisible = setting;
        dayUI.SetActive(dayIsVisible);
    }
    public void ToggleDay()
    {
        dayIsVisible = !dayIsVisible;
        ToggleDay(dayIsVisible);
    }
    public void UpdateDay()
    {
        string s = "Day: " + GameManager.Instance.CurrentDay + ", Time of Day: " + GameManager.Instance.CurrentTimeOfDay;
        s += "\nIs Busy? " + GameManager.Instance.IsOpen();
        dayText.text = s;
    }
    void ToggleVar(bool setting)
    {
        varIsVisible = setting;
        varUI.SetActive(setting);
    }
    public void ToggleVar()
    {
        varIsVisible = !varIsVisible;
        varUI.SetActive(varIsVisible);
    }
    public void SetDay(int day)
    {
        if (timeManager == null)
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
        }
        timeManager.SetDay(day.ToString());
    }
    public void SetTimeOfDay(string param)
    {
        if (timeManager == null)
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
        }
        timeManager.SetTimeOfDay(param);
    }
    public void SetLocation(string scene)
    {
        LevelLoader.Instance.GoTo(scene);
    }
}
