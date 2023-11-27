using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Talker
{
    public override void Interact()
    {
        base.Interact();
        gameObject.SetActive(false);
    }
}
