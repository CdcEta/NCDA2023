using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSetActive : MonoBehaviour
{
    public GameObject[] openGameObjects;
    public GameObject[] closeGameObjects;
    // Start is called before the first frame update

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(var i in openGameObjects)
            {
                i.SetActive(true);
            }
        }
    }
}
