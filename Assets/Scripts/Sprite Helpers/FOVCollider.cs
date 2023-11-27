using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVCollider : MonoBehaviour
{
    public bool seesPlayer;
    PolygonCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        seesPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        seesPlayer = false;
    }
}
