using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static EasyEvent<bool> unlockEvent = new EasyEvent<bool>();
    // Start is called before the first frame update
    void Start()
    {
        unlockEvent.Subscribe(true, Print);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            new Achievement("test", false).Unlock();
        }
    }
    void Print()
    {
        Debug.Log("worked!");
    }
}

public class Achievement 
{ 
    public string Name { get; private set; }
    public bool IsUnlocked { get; private set; }
    public Achievement(string name, bool unlocked)
    {
        Name = name;
        IsUnlocked = unlocked;
    }
    public void Unlock()
    {
        AchievementManager.unlockEvent.Invoke();
    }

}

