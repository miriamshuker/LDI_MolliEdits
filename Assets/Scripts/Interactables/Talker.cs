using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Talker : Interactable
{
    //Reference to phone UI, to be able to send text notifications?
    DialogueRunner dialogueRunner;
    public string nodeName;
    public ScheduledDialogue[] scheduledDialogues;

    [Header("Optional")]
    public YarnProgram scriptToLoad;

    protected override void Start()
    {
        base.Start();
        dialogueRunner = GameDialogueManager.Instance.dr;

        if (scriptToLoad != null)
        {
            Debug.Log("Adding script " + scriptToLoad.name);

            dialogueRunner.Add(scriptToLoad);
        }
    }

    public override void Interact()
    {
        if (nodeName.Length > 0)
        {
            Debug.Log(dialogueRunner != null);
            dialogueRunner.StartDialogue(nodeName);
            return;
        }
        foreach (ScheduledDialogue s in scheduledDialogues)
        {
            if (s.timeSlot.CheckSlot())
            {
                dialogueRunner.StartDialogue(s.nodeName);
            }
        }
    }

    [YarnCommand("setactive")]
    public void SetActive(string param)
    {
        bool.TryParse(param, out bool setting);
        enabled = setting;
    }

    [YarnCommand("showsprite")]
    public void ShowSprite(string param)
    {
        bool.TryParse(param, out bool setting);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.enabled = setting;
        }
    }
    [YarnCommand("animatesprite")]
    public void AnimateSprite(string[] param)
    {
        if (param.Length != 2)
        {
            Debug.Log("animatesprite call doesn't have 2 parameters");
            return;
        }
        bool.TryParse(param[1], out bool setting);
        Animator a = GetComponent<Animator>();
        if (a)
        {
            a.SetBool(param[0], setting);
        }
    }
}
