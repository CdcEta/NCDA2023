using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSetActive : MonoBehaviour
{
    public bool setByInteraction;
    public GameObject[] openGameObjects;
    public GameObject[] closeGameObjects;
    public bool isInTrigger;
    // Start is called before the first frame update

    private void Update()
    {
        if (isInTrigger)
        {
            if (setByInteraction)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    foreach (var i in openGameObjects)
                    {
                        i.SetActive(true);
                    }
                    foreach (var i in closeGameObjects)
                    {
                        i.SetActive(false);
                    }
                }
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("Player"))
        {
            isInTrigger = true;
            if (!setByInteraction)
            {
                foreach (var i in openGameObjects)
                {
                    i.SetActive(true);
                }
                foreach (var i in closeGameObjects)
                {
                    i.SetActive(false);
                }
            }
       
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isInTrigger = false;
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
