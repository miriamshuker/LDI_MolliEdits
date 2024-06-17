using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;

public class QuickDebug : MonoBehaviour
{
    public Button button;
    public GameObject menu;
    public Clock clock;
    private bool isVisible;
    private bool isOpen;
    private TimeManager timeManager;
    private LevelLoader levelLoader;

    private void Start()
    {
        SetVisible(GameManager.Instance.debugMode);
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
    private void Update()
    {
        Keyboard k = Keyboard.current;
        if (k.ctrlKey.isPressed && k.tabKey.wasReleasedThisFrame)
        {
            ToggleVisible();
        }
    }
    public void SelectQuickDialogue(bool setting)
    {
        GameManager.Instance.enteringNodeName = setting;
    }
    void ToggleVisible()
    {
        isVisible = !isVisible;
        button.gameObject.SetActive(isVisible);
        GameManager.Instance.debugMode = isVisible;
        isOpen = false;
        menu.SetActive(false);
    }
    void SetVisible(bool setting)
    {
        isVisible = setting;
        button.gameObject.SetActive(isVisible);
        GameManager.Instance.debugMode = isVisible;
        isOpen = false;
        menu.SetActive(false);
    }
    public void Toggle()
    {
        isOpen = !isOpen;
        menu.SetActive(isOpen);
    }
    public void QuickJump(string nodeName)
    {
        DialogueRunner dr = GameDialogueManager.Instance.dr;
        dr.StartDialogue(nodeName);
        Toggle();
    }
    public void ToDay(int day)
    {
        FindHelpers();
        switch (day)
        {
            case 0:
                timeManager.Day0(); break;
            case 1:
                timeManager.Day1(); break;
            case 2:
                timeManager.Day2(); break;
            case 3:
                timeManager.Day3(); break;
        }
    }
    public void Day0Dinner()
    {
        timeManager.Set(0, GameManager.TimeOfDay.DINNER);
        clock.SetTimeAndStart("20:30", "22:00", 5);
        levelLoader.GoTo("HomeLower", true);
    }
    public void Day0Bedtime()
    {
        timeManager.Set(0, GameManager.TimeOfDay.BEDTIME);
        clock.SetTimeAndStart("22:00", "23:00", 0);
        levelLoader.GoTo("EddyRoom", true);
    }
    public void Day1Dinner()
    {
        timeManager.Set(0, GameManager.TimeOfDay.DINNER);
        clock.SetTimeAndStart("20:30", "22:00", 5);
        levelLoader.GoTo("HomeLower", true);
    }
}
