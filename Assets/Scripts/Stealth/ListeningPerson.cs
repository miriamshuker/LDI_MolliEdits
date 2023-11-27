using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListeningPerson : MonoBehaviour, ISoundListener
{
    public enum AlertState
    {
        GREEN,
        YELLOW,
        RED
    }
    public AlertState alertState;
    public float softRadius; //reacts with suspicion
    public float hardRadius; //reacts with alarm
    public bool isDeaf;
    public float calmTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ListenSound(Vector3 soundPos)
    {
        if (!isDeaf)
        {
            float dist = Vector3.Distance(transform.position, soundPos);
            if (dist <= softRadius)
            {
                StopAllCoroutines();
                if (dist <= hardRadius)
                {
                    Debug.Log("Heard an alarming sound!");
                    alertState = AlertState.RED;

                    StartCoroutine(CalmDown());
                }
                else
                {
                    Debug.Log("Heard a suspicious sound!");
                    alertState = AlertState.YELLOW;

                    StartCoroutine(CalmDown());
                }
            }
            else
            {
                Debug.Log("Heard a distant sound");
            }
        }
    }
    public void Deafen()
    {

    }
    public void Undeafen()
    {

    }
    IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(calmTime);
        alertState = AlertState.GREEN;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(softRadius, softRadius));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(hardRadius, hardRadius));
    }
}
