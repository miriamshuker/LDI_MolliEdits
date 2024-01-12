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
    private bool isVisible;
    private bool isOpen;

    private void Start()
    {
        SetVisible(GameManager.Instance.debugMode);
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
}
