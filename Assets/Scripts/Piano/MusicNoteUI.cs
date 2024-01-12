using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicNoteUI : MonoBehaviour
{
    public Image note, ledgerLine;
    public Image holeWhole, holeHalf, lineQuarter, lineEighth;
    public RectTransform rect;
    public float extraScaling;
    public void SetPosition(float x, float yOffset, float ySpacing, int noteNum, bool useSharps)
    {
        if (MusicNote.IsBlackKey((MusicKey)noteNum))
        {
            if (useSharps)
            {
                noteNum -= 1;
            }
            else
            {
                noteNum += 1;
            }
        }
        //adjust for octave
        int octave = noteNum / 12;
        noteNum += octave * 2;

        //convert from music note to staff note
        if (noteNum % 2 == 0)
        {
            noteNum = noteNum / 2;
        }
        else
        {
            noteNum = (noteNum + 1) / 2;
        }
        rect.anchoredPosition = new Vector2(x, yOffset + ySpacing * noteNum);
    }
}
