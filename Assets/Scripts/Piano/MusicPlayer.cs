using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public InputHelper input;
    public AudioSource[] keyAudio;
    public Button[] keyButton;
    public Color blackNorm, blackPress;
    public Color whiteNorm, whitePress;

    // Start is called before the first frame update
    void Start()
    {
        InputAction[] keys = input.pianoKeys;
        if (keyAudio.Length != keys.Length || keyAudio.Length != keyButton.Length)
        {
            Debug.LogWarning("Piano is messed up!");
            return;
        }
        whiteNorm = keyButton[0].colors.normalColor;
        whitePress = keyButton[0].colors.pressedColor;
        blackNorm = keyButton[1].colors.normalColor;
        blackPress = keyButton[1].colors.pressedColor;
        for (int i = 0; i < keys.Length; i++)
        {
            int _i = i;
            keys[i].started += _ => Highlight(_i);
            keys[i].canceled += _ => Play(_i);
            keyButton[i].onClick.AddListener(() => Play(_i));
        }
    }
    void Play(int num)
    {
        keyAudio[num].Play();
        SetButtonAlpha(num, 1f);
    }
    void Highlight(int num)
    {
        SetButtonAlpha(num, 0.75f);
    }
    void SetButtonAlpha(int num, float alpha)
    {
        Color c = keyButton[num].image.color;
        c.a = alpha;
        keyButton[num].image.color = c;
    }
}
