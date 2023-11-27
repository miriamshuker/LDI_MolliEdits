using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour
{
    public ObjectSchedule schedule;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.debugMode)
            return;
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            ToggleDay();
        }
        if (dayIsVisible)
        {
            UpdateDay();
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket)) {
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
        s += "\nIs Busy? " + GameManager.Instance.isBusy;
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
        schedule.SetDay(day.ToString());
    }
    public void SetTimeOfDay(string param)
    {
        schedule.SetTimeOfDay(param);
    }
}
