using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SequentialObjectPlacer : MonoBehaviour
{
    bool hasItem;
    public GameObject itemSource;
    public GameObject[] objectsToPlace;
    int itemsPlaced;

    [System.Serializable]
    public class ObjectPlacement
    {
        public GameObject item;
        public Vector3 position;

        public void PlaceObject()
        {
            item.transform.position = position;
        }
    }

    private void Start()
    {
        foreach (GameObject g in objectsToPlace)
        {
            //Debug.Log(g.name);
            g.SetActive(false);
        }
    }

    [YarnCommand("seqpickup")]
    public void Pickup()
    {
        //Debug.Log("Bowl should be invisible");
        hasItem = true;
        itemSource.SetActive(false);
    }
    [YarnCommand("seqplace")]
    public void Place(string param)
    {
        //Debug.Log("Bowl should be visible again");
        int.TryParse(param, out int index);
        hasItem = false;
        if (itemsPlaced < objectsToPlace.Length)
        {
            objectsToPlace[index].SetActive(true);
            itemsPlaced++;
        }
    }
    [YarnCommand("seqshow")]
    public void ShowSource()
    {
        itemSource.SetActive(true);
    }
}
