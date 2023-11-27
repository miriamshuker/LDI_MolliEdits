using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [System.Serializable]
    public class MusicMeasure
    {
        public List<MusicNote> notes;
        public int timeSignature = 4;
        public bool isPickup;
        public int MeasureNumber
        {
            get;
            set;
        }
        public int CurrentIndex
        {
            get;
            set;
        }
        public bool IsFinished
        {
            get;
            set;
        }
        public void NextNote()
        {
            CurrentIndex += 1;
            if (CurrentIndex >= notes.Count)
            {
                 CurrentIndex = notes.Count;
                 IsFinished = true;
            }
        }
    }

    public string pieceName;
    public List<MusicMeasure> measures;

    public int CurrentIndex
    {
        get;
        set;
    }
    public bool IsFinished
    {
        get;
        set;
    }
    public void NextMeasure()
    {
        CurrentIndex += 1;
        if (CurrentIndex >= measures.Count)
        {
            CurrentIndex = measures.Count;
            IsFinished = true;
        }
    }
}
