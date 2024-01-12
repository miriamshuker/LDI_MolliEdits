using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoKey : MonoBehaviour
{
    public MusicKey key;
    public AudioSource sound;
    public Button button;
    public Color normalColorW, pressColorW;
    public Color normalColorB, pressColorB;

    public void Play()
    {
        sound.Play();
    }
    public void PlayAndHighlight()
    {
        sound.Play();
        SetAlpha(0.75f);
    }
    public void Release()
    {
        SetAlpha(1f);
    }
    void SetAlpha(float alpha)
    {
        Color c = button.image.color;
        c.a = alpha;
        button.image.color = c;
    }
    public void SetColor(Color normalW, Color pressW, Color normalB, Color pressB)
    {
        normalColorW = normalW;
        pressColorW = pressW;
        normalColorB = normalB;
        pressColorB = pressB;
        PaintColor();
    }
    void PaintColor()
    {
        ColorBlock block = button.colors;
        block.normalColor = MusicNote.IsBlackKey(key) ? normalColorB : normalColorW;
        block.pressedColor = MusicNote.IsBlackKey(key) ? pressColorB : pressColorW;
        block.highlightedColor = MusicNote.IsBlackKey(key) ? pressColorB : pressColorW;
        button.colors = block;
    }
}
