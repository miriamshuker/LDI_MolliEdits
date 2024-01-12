using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSheet : MonoBehaviour
{
    [System.Serializable]
    public class MusicMeasure
    {
        public int timeSignature = 4;
        public bool isPickup;
        public List<MusicNote> notes;
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
        public bool TryKey(MusicKey testKey)
        {
            if (notes == null || testKey == notes[CurrentIndex].noteKey)
            {
                NextNote();
                return true;
            }
            return false;
        }
        public void NextNote()
        {
            CurrentIndex += 1;
            Debug.Log(MeasureNumber + " " + CurrentIndex);
            if (CurrentIndex >= notes.Count)
            {
                CurrentIndex = notes.Count;
                IsFinished = true;
            }
        }
        public void Restart()
        {
            CurrentIndex = 0;
            IsFinished = false;
        }
    }

    public string pieceName;
    public bool useSharps;
    public List<MusicMeasure> measures;

    private void Start()
    {
        for (int i = 0; i < measures.Count; i++)
        {
            measures[i].MeasureNumber = i;
        }
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
    public void NextMeasure()
    {
        CurrentIndex += 1;
        Debug.Log("On measure " + CurrentIndex);
        if (CurrentIndex >= measures[CurrentIndex].notes.Count)
        {
            CurrentIndex = measures.Count;
            IsFinished = true;
            Debug.Log("Finished piece!");
        }
    }
    public void TryKey(MusicKey lastKey)
    {
        bool test = measures[CurrentIndex].TryKey(lastKey);
        Debug.Log(lastKey + " is " + test);
        if (test && measures[CurrentIndex].IsFinished)
        {
            NextMeasure();
        }
    }
    void Restart()
    {
        CurrentIndex = 0;
        IsFinished = false;
        foreach (MusicMeasure m in measures)
        {
            m.Restart();
        }
    }
}
