using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : Interactable
{
    public string sceneToLoad;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        LevelLoader.Instance.GoTo(sceneToLoad);
    }
}
