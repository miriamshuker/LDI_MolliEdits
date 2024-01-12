using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[System.Serializable]
public class NoteList {
    public Note[] notes;
}

public class JsonReader : MonoBehaviour
{
    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        ReadJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ReadJson()
    {
        Debug.Log("reading...");
        Assert.IsNotNull(jsonFile);
        Debug.Log(jsonFile.text);
        NoteList notes = JsonUtility.FromJson<NoteList>(jsonFile.text);
        foreach (Note n in notes.notes)
        {
            Debug.Log(n.header);
        }
    }
}
