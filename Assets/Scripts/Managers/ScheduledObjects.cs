using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScheduledObjects : MonoBehaviour
{
    public string scheduleName;
    public List<GameObject> gameObjects;
    public ObjectSchedule.TimeSlot[] timeSlots;

    public void SetActive(bool setting)
    {
        foreach (GameObject g in gameObjects)
        {
            //Debug.Log("setting " + g.name + " to " + setting);
            g.SetActive(setting);
        }
    }
}
