using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class SecurityManager : MonoBehaviour
{
    //Has a static instance.
    //Has an image or display to show the current security level, yellow to red. Also has variables to control this amount.
    //void AddThreat, called when the player is left in a security camera's view for too long or gets touched by a laser.
    //void SubtractThreat, called when undetected for long enough.
    //void CatchPlayer, called when threat limit is surpassed. Triggers a short cutscene, then respawns the player at the previous checkpoint.
    public int threatSteps;
    public float threatPerStep;
    float currentThreat;
    int currentThreatStep;
    public Image threatMeter;
    public float relaxTime;
    public float relaxAmount;

    public string caughtNodeName;
    public Transform defaultCheckpoint;
    Vector3 currentCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        currentCheckpoint = defaultCheckpoint.position;
    }

    public void AddThreat(float threatAmount)
    {
        StopAllCoroutines();
        currentThreat += threatAmount;
        Debug.Log("+" + threatAmount);
        if (currentThreat >= threatPerStep)
        {
            Debug.Log("going next level");
            threatAmount = 0;
            currentThreatStep++;
            UpdateMeter();
            if (currentThreatStep > threatSteps)
            {
                CatchPlayer();
            }
        }
        SubtractThreat();
    }
    public void SubtractThreat()
    {
        StartCoroutine(Relax());
    }
    IEnumerator Relax()
    {
        yield return new WaitForSeconds(relaxTime);
        currentThreat -= Mathf.Clamp(relaxTime, 0, threatPerStep);
    }
    void UpdateMeter()
    {
        threatMeter.fillAmount = (currentThreatStep / (float)threatSteps);
        //threatMeter.transform.localScale = new Vector3(1 + (0.1f * currentThreatStep), 1 + (0.1f * currentThreatStep), 1);
    }
    public void CatchPlayer()
    {
        FindObjectOfType<DialogueRunner>().StartDialogue(caughtNodeName);
    }
    [YarnCommand("spawncaught")]
    public void SpawnCaughtPlayer()
    {

    }
    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        currentCheckpoint = spawnPoint;
    }
}
