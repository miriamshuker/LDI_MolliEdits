using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class YarnItem : MonoBehaviour
{
    public string id;
    public GameObject item;
    Animator anim;
    SpriteRenderer sr;

    // Start is called before the first frame update
    private void Awake()
    {
        if (item == null)
            item = this.gameObject;
        anim = item.GetComponent<Animator>();
        sr = item.GetComponent<SpriteRenderer>();
    }
    
    public void Show(bool setting)
    {
        Debug.Log("trying to show");
        if (item)
            item.SetActive(setting);
        else
            gameObject.SetActive(setting);
    }
    public void Animate(string state)
    {
        anim.Play(state);
    }
}
