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
    public void Animate(string state)
    {
        Debug.Log($"animate? {state}");
        foreach (YarnItem y in items)
        {
            if (y != null && y.id == state)
            {
                y.Show(true);
                y.Animate(state);
                Debug.Log("animating");
            }
        }
    }
}
