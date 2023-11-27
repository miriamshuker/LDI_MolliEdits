using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteSwap))]
public class SecurityCamera : MonoBehaviour
{
    public float threatLevel;
    public float courtesyTime;
    float timeSeen;

    SecurityManager sm;

    public bool isOn;
    public SpriteSwap spriteSwap;
    public SpriteRenderer otherSr;
    public Transform toRotate;
    public FieldOfView fov;
    public FOVCollider col;

    public Color okColor;
    public Color noColor;

    public float pivotTime;
    public float delayTime;
    public float waitTime;
    public float angleIncrement;
    public SecurityDirection initialDirection;
    public bool startsMovingRight;
    public bool goesBackAndForth;

    Vector3 initialEulerAngles;
    int index;
    bool reversing;
    bool seesPlayer;

    public enum SecurityDirection
    {
        L,
        C,
        R,
    }

    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SecurityManager>();

        initialEulerAngles = toRotate.eulerAngles;
        index = (int)initialDirection;
        RotateFov(index);
        StartCoroutine(WaitStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (col.seesPlayer)
        {
            otherSr.color = noColor;
            timeSeen += Time.deltaTime;
            if (timeSeen > courtesyTime)
            {
                timeSeen = 0;
                sm.AddThreat(threatLevel);
            }
        }
        else
        {
            otherSr.color = okColor;
        }
        
    }
    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(PivotCamera());
    }
    IEnumerator PivotCamera()
    {
        while (isOn)
        {
            yield return new WaitForSeconds(pivotTime);
            //spriteSwap.SwapSpritePend();
            RotateFov();
            if (delayTime > 0)
            {
                yield return DelayCamera();
            }
        }
    }
    IEnumerator DelayCamera()
    {
        yield return new WaitForSeconds(delayTime);
    }
    void RotateFov()
    {
        //Debug.Log(index);
        if (index == 2)
        {
            reversing = true;
        }
        else if (index == 0)
        {
            reversing = false;
        }
        if (goesBackAndForth)
        {
            if (reversing)
            {
                if (goesBackAndForth)
                {
                    index--;
                }
            }
            else
            {
                index++;
            }
        }
        else
        {
            if (startsMovingRight)
            {
                index++;
                if (index > 2)
                {
                    index = 0;
                }
            }
            else
            {
                index--;
                if (index < 0)
                {
                    index = 2;
                }
            }
        }
        Debug.Log(index);
        switch (index)
        {
            case 0:
                toRotate.eulerAngles = initialEulerAngles + new Vector3(0, 0, -angleIncrement);
                break;
            case 1:
                toRotate.eulerAngles = initialEulerAngles;
                break;
            case 2:
                toRotate.eulerAngles = initialEulerAngles + new Vector3(0, 0, angleIncrement);
                break;
        }
        spriteSwap.SwapSprite(index);
    }
    void RotateFov(int ind)
    {
        switch (index)
        {
            case 0:
                toRotate.eulerAngles = initialEulerAngles + new Vector3(0, 0, -angleIncrement);
                break;
            case 1:
                toRotate.eulerAngles = initialEulerAngles;
                break;
            case 2:
                toRotate.eulerAngles = initialEulerAngles + new Vector3(0, 0, angleIncrement);
                break;
        }
        spriteSwap.SwapSprite(index);
    }
}
