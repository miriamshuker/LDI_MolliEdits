using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public InputHelper input;
    public PianoKey[] pianoKeys;
    public Color blackNorm, blackPress;
    public Color whiteNorm, whitePress;
    public int maxKeys;
    private int pressedKeys;

    public MusicSheet currentPiece;
    public MusicNoteUI noteUI;
    public RectTransform sheetTransform;
    public float vSpacing, hSpacing, cOffset, xOffset;

    // Start is called before the first frame update
    void Start()
    {
        InputAction[] keys = input.pianoKeys;
        if (pianoKeys == null || pianoKeys.Length != 13)
        {
            Debug.LogWarning("Piano is messed up!");
            return;
        }

        for (int i = 0; i < keys.Length; i++)
        {
            int _i = i;
            keys[i].started += _ => PlayAndHighlight(_i);
            keys[i].canceled += _ => Release(_i);
            pianoKeys[i].button.onClick.AddListener(() => Play(_i));
            pianoKeys[i].SetColor(whiteNorm, whitePress, blackNorm, blackPress);
        }

        vSpacing = sheetTransform.rect.height / 4;
    }
    void PlayAndHighlight(int num)
    {
        Increment(num);
        pianoKeys[num].PlayAndHighlight();
    }
    void Play(int num)
    {
        Increment(num);
        pianoKeys[num].Play();
    }
    void Release(int num)
    {
        Decrement(num);
        pianoKeys[num].Release();
    }
    void Increment(int num)
    {
        pressedKeys += 1;
        noteUI.SetPosition(xOffset, cOffset, vSpacing, num, currentPiece.useSharps);
    }
    void Decrement(int num)
    {
        pressedKeys -= 1;
        if (currentPiece != null && !currentPiece.IsFinished && pressedKeys == 0)
        {
            currentPiece.TryKey((MusicKey)num);
        }
    }
}
