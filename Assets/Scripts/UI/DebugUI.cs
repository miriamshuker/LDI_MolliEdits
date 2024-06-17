using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DebugUI : MonoBehaviour
{
    public TimeManager timeManager;
    public LevelLoader levelLoader;
    public Clock clock;
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
        FindHelpers();
    }
    void FindHelpers()
    {
        if (levelLoader == null)
        {
            levelLoader = GameObject.FindObjectOfType<LevelLoader>();
        }
        if (timeManager == null)
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
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
        timeManager.SetDay(day.ToString());
    }
    public void SetTimeOfDay(string param)
    {
        timeManager.SetTimeOfDay(param);
    }
    public void SetLocation(string scene)
    {
        LevelLoader.Instance.GoTo(scene);
    }
}
