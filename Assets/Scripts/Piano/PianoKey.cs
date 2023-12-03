using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoKey : MonoBehaviour
{
    public MusicKey key;
    public AudioSource sound;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Play()
    {
        sound.Play();
        SetButtonColor(1f);
    }
    public void Highlight()
    {
        SetButtonColor(0.75f);
    }
    void SetButtonColor(float alpha)
    {
        Color c = button.image.color;
        c.a = alpha;
        button.image.color = c;
    }
}
