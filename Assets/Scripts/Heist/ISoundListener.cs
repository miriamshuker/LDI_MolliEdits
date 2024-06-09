using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundListener
{
    void ListenSound(Vector3 soundPos);
    void Deafen();
    void Undeafen();
}
