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
    [System.Serializable]
    public class SpriteInfo
    {
        public string name;
        public Sprite sprite;
    }
    public SpriteInfo[] sprites;

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
        if (anim)
        {
            anim.Play(state);
        }
    }
    public void ShowSprite(string sprite)
    {
        foreach (SpriteInfo si in sprites)
        {
            if (si.name == sprite)
            {
                sr.sprite = si.sprite;
                return;
            }
        }
    }
    public void EnableSprite(bool setting)
    {
        sr.enabled = setting;
    }
}
