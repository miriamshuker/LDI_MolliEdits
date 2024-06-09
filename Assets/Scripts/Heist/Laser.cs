using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Laser : Talker
{
    public float threatLevel;
    public bool moves;
    public bool blinks;
    public bool moveToward1;
    public float moveSpeed;
    public Vector3 endPoint1;
    public Vector3 endPoint2;
    public float upTime;
    public float downTime;
    SpriteRenderer sr;
    BoxCollider2D col;
    SecurityManager sm;
    public DialogueRunner dr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        sm = FindObjectOfType<SecurityManager>();
        
        StartCoroutine(BlinkLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if (moves)
        {
            if (moveToward1)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint1, Time.deltaTime * moveSpeed);
                //transform.position = Vector3.Lerp(endPoint2, endPoint1, Time.deltaTime * moveSpeed);
                if (Vector3.Distance(transform.position, endPoint1) < 0.1f)
                {
                    moveToward1 = !moveToward1;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint2, Time.deltaTime * moveSpeed);
                if (Vector3.Distance(transform.position, endPoint2) < 0.1f)
                {
                    moveToward1 = !moveToward1;
                }
            }
        }
    }

    IEnumerator BlinkLaser()
    {
        while (blinks)
        {
            
            yield return new WaitForSeconds(upTime);
            if (downTime > 0)
            {
                SetLaser(false);
                yield return new WaitForSeconds(downTime);
                SetLaser(true);
            }
        }
    }
    void SetLaser(bool setting)
    {
        sr.enabled = setting;
        col.enabled = setting;
    }
    public override void Interact()
    {
        if (nodeName.Length > 0)
        {
            dr.StartDialogue(nodeName);
        }
    }
    private new void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(endPoint1, 0.1f);
        Gizmos.DrawWireSphere(endPoint2, 0.1f);
    }
}
