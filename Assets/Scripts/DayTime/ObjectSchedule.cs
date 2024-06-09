using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ObjectSchedule : MonoBehaviour
{
    [System.Serializable]
    public class TimeSlot
    {
        public string timeName;
        [Range(0, 4)]
        public int day;
        public GameManager.TimeOfDay timeOfDay;

        public bool CheckSlot()
        {
            return (day == GameManager.Instance.CurrentDay && timeOfDay == GameManager.Instance.CurrentTimeOfDay);
        }
    }
    public bool useStart;
    public TimeSlot startSlot;
    public ScheduledObjects[] scheduledObjects;
    public ScheduledDialogue[] scheduledDialogues;
    private DialogueRunner dr;

    // Start is called before the first frame update
    void Start()
    {
        dr = FindObjectOfType<DialogueRunner>();
        if (useStart)
        {
            GameManager.Instance.SetDay(startSlot.day);
            GameManager.Instance.SetTimeOfDay(startSlot.timeOfDay);
        }
        LoadSchedule();
        LoadDialogue();
    }
    public void LoadSchedule()
    {
        foreach (ScheduledObjects s in scheduledObjects)
        {
            foreach (TimeSlot t in s.timeSlots)
            {
                if (t != null)
                {
                    bool check = t.CheckSlot();
                    //Debug.Log(s.scheduleName + "'s " + t.timeName + " current time slot found? " + check);
                    if (check)
                    {
                        //Debug.Log(t.timeName + " should be active");
                        s.SetActive(true);
                        break;
                    }
                    else
                    {
                        s.SetActive(false);
                    }
                }
            }
        }
    }
    public void LoadDialogue()
    {
        foreach (ScheduledDialogue d in scheduledDialogues)
        {
            if (d != null)
            {
                //Debug.Log(d.nodeName + d.timeSlot.CheckSlot() + "Dr?" + (dr != null));
                if (d.timeSlot.CheckSlot())
                {
                    dr.StartDialogue(d.nodeName);
                }
            }
        }
    }
    [YarnCommand("setday")]
    public void SetDay(string param)
    {
        int.TryParse(param, out int day);
        if (day <= 4 && day >= 0)
        {
            GameManager.Instance.SetDay(day);
        }
        LoadSchedule();
        LoadDialogue();
    }
    [YarnCommand("settimeofday")]
    public void SetTimeOfDay(string param)
    {
        //Debug.Log("Trying to find time of day " + param);
        GameManager.TimeOfDay timeOfDay;
        switch (param)
        {
            case "morning":
                timeOfDay = GameManager.TimeOfDay.MORNING;
                break;
            case "afternoon":
                timeOfDay = GameManager.TimeOfDay.AFTERNOON;
                break;
            case "freetime":
                timeOfDay = GameManager.TimeOfDay.FREETIME;
                break;
            case "evening":
                timeOfDay = GameManager.TimeOfDay.EVENING;
                break;
            case "dinner":
                timeOfDay = GameManager.TimeOfDay.DINNER;
                break;
            case "night":
                timeOfDay = GameManager.TimeOfDay.NIGHT;
                break;
            case "bedtime":
                timeOfDay = GameManager.TimeOfDay.BEDTIME;
                break;
            case "between":
                timeOfDay = GameManager.TimeOfDay.BETWEEN;
                break;
            default:
                Debug.Log("No matching time of day found");
                return;
        }
        GameManager.Instance.SetTimeOfDay(timeOfDay);
        LoadSchedule();
        LoadDialogue();
    }
}
