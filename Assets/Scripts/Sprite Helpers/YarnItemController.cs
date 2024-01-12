using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Yarn.Unity;

public class YarnItemController : MonoBehaviour
{
    private List<YarnItem> items = new List<YarnItem>();

    private void Start()
    {
        YarnItem[] find = FindObjectsOfType<YarnItem>(true);
        foreach (YarnItem y in find)
        {
            items.Add(y);
        }

        StringBuilder sb = new StringBuilder();
        foreach (YarnItem y in items)
        {
            sb.Append($"{y.id}|");
        }
        Debug.Log(sb.ToString());
    }
    public void Add(YarnItem y)
    {
        items.Add(y);
    }
    [YarnCommand("yarnon")]
    public void Show(string itemName)
    {
        Debug.Log("show?");
        foreach (YarnItem y in items)
        {
            if (y != null && y.id == itemName)
            {
                y.Show(true);
                Debug.Log("showing");
            }
        }
    }
    [YarnCommand("yarnoff")]
    public void Hide(string itemName)
    {
        Debug.Log("hide?");
        foreach (YarnItem y in items)
        {
            if (y != null && y.id == itemName)
            {
                y.Show(false);
                Debug.Log("hiding");
            }
        }
    }
    [YarnCommand("yarnanim")]
    public void Animate(string[] param)
    {
        string id = param[0];
        string state = param[1];
        Debug.Log($"animate? {state}");
        foreach (YarnItem y in items)
        {
            if (y != null && y.id == id)
            {
                y.Show(true);
                y.Animate(state);
                Debug.Log("animating");
            }
        }
    }
    [YarnCommand("yarnsprite")]
    public void SetSprite(string[] param)
    {
        string itemName = param[0];
        string spriteName = param[1];
        foreach (YarnItem y in items)
        {
            if (y != null && y.id == itemName)
            {
                y.ShowSprite(spriteName);
                Debug.Log("spriting");
            }
        }
    }
    [YarnCommand("yarnenable")]
    public void EnableSprite(string[] param)
    {
        string itemName = param[0];
        string setting = param[1];
        foreach (YarnItem y in items)
        {
            if (y != null && y.id == itemName)
            {
                y.EnableSprite(setting == "true");
                Debug.Log("spriting");
            }
        }
    }
    [YarnCommand("yarnlight")]
    public void SetLightSwitch(bool on)
    {
        foreach (YarnItem y in items)
        {
            if (y != null && y is YarnLight light)
            {
                light.SetLights(on);
            }
        }
    }
}
