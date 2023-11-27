using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TimeManager : MonoBehaviour
{
    public Clock clock;
    public Day any;
    public Day[] days;
    public bool ignoreGameTime;
    [Range(0, 3)]
    public int day;
    public GameManager.TimeOfDay timeOfDay;
    bool began;

    private void Start()
    {
        Debug.Log($"{GameManager.Instance.CurrentDay} {GameManager.Instance.CurrentTimeOfDay}");
        if (ignoreGameTime)
        {
            SetDay(day);
            SetTimeOfDay(timeOfDay);
        }
        PhoneManager.Instance.SetTimeManager(this);
    }
    private void Update()
    {

        if (!GameManager.Instance.debugMode || GameManager.Instance.enteringNodeName)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetDay(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetDay(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetDay(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetDay(3);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PrevTimeOfDay();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            NextTimeOfDay();
        }
    }
    public void Begin()
    {
        SetDay(GameManager.Instance.CurrentDay);

        SetTimeOfDay(GameManager.Instance.CurrentTimeOfDay);
        if (!ignoreGameTime)
            SetTimeOfDay(GameManager.Instance.CurrentTimeOfDay);

        Debug.Log("beginning");
    }
    public void PlayPreTransition()
    {
        days[GameManager.Instance.CurrentDay].PlayPreTransitionNode(GameManager.Instance.CurrentTimeOfDay);
    }
    [YarnCommand("settime")]
    public void SetTime(string[] param)
    {
        if (param.Length != 2)
        {
            Debug.LogWarning("not enough params!");
            return;
        }
        int.TryParse(param[0], out int num);
        Set(num, param[1]);
    }
    void Set(int day, string tod)
    {
        SetDay(day);
        SetTimeOfDay(tod);
    }
    void Set(int day, GameManager.TimeOfDay tod)
    {
        SetDay(day);
        SetTimeOfDay(tod);
    }
    [YarnCommand("setday")]
    public void SetDay(string param)
    {
        int.TryParse(param, out int num);
        SetDay(num);
    }
    void SetDay(int num)
    {
        if (num < days.Length)
        {
            GameManager.Instance.SetDay(num);
            for (int i = 0; i < days.Length; i++)
            {
                days[i].HideAll();
                if (i == num)
                {
                    days[i].gameObject.SetActive(true);

                    Debug.Log($"Day: {i}");
                }
                else
                {
                    days[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError($"day { num } doesn't exist! (max size is { days.Length })");
        }
    }
    [YarnCommand("settod")]
    public void SetTimeOfDay(string param)
    {
        GameManager.TimeOfDay tod;

        switch (param)
        {
            case "morning":
                tod = GameManager.TimeOfDay.MORNING;
                break;
            case "afternoon":
                tod = GameManager.TimeOfDay.AFTERNOON;
                break;
            case "freetime":
                tod = GameManager.TimeOfDay.FREETIME;
                break;
            case "evening":
                tod = GameManager.TimeOfDay.EVENING;
                break;
            case "dinner":
                tod = GameManager.TimeOfDay.DINNER;
                break;
            case "night":
                tod = GameManager.TimeOfDay.NIGHT;
                break;
            case "bedtime":
                tod = GameManager.TimeOfDay.BEDTIME;
                break;
            case "between":
                tod = GameManager.TimeOfDay.BETWEEN;
                break;
            default:
                Debug.Log("No matching time of day found");
                return;
        }

        GameManager.Instance.SetTimeOfDay(tod);

        days[GameManager.Instance.CurrentDay].ShowTimeOfDay(tod);
        any.ShowTimeOfDay(tod);

        Debug.Log($"Time of day: {tod}");
    }
    public void SetTimeOfDay(GameManager.TimeOfDay tod)
    {
        GameManager.Instance.SetTimeOfDay(tod);
        Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        
        any.ShowTimeOfDay(tod);
        days[GameManager.Instance.CurrentDay].ShowTimeOfDay(tod);

        Debug.Log($"Time of day: {tod}");
    }
    void PrevTimeOfDay()
    {
        GameManager.TimeOfDay tod = GameManager.Instance.CurrentTimeOfDay;
        if (tod > 0)
        {
            tod--;
            GameManager.Instance.SetTimeOfDay(tod);
        }
        days[GameManager.Instance.CurrentDay].ShowTimeOfDay(tod);
        any.ShowTimeOfDay(tod);
        Debug.Log($"Time of day: {tod}");
    }
    [YarnCommand("nexttod")]
    public void NextTimeOfDay()
    {
        Debug.Log("next tod");
        GameManager.TimeOfDay tod = GameManager.Instance.CurrentTimeOfDay;
        if (tod < GameManager.TimeOfDay.BETWEEN)
        {
            tod++;
            GameManager.Instance.SetTimeOfDay(tod);
        }
        days[GameManager.Instance.CurrentDay].ShowTimeOfDay(tod);
        any.ShowTimeOfDay(tod);
        Debug.Log($"Time of day: {tod}");
    }
    public void SetState(bool isOn)
    {
        clock.SetState(isOn);
    }
}
