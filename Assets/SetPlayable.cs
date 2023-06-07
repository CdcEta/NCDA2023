using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SetPlayable : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public bool setByInteraction;
    public bool setByTrigger;
    private bool isInTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTrigger)
        {
           
            
                Interaction();
            
            gameObject.SetActive(false);
        }
    }

    private void Interaction()
    {
        if (setByInteraction && Input.GetKeyDown(KeyCode.F))
        {
            playableDirector.Play();
        }
    }
    private void Trigger()
    {
        if (setByTrigger)
        {
            playableDirector.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
           
        if (collision.CompareTag("Player"))
        {
                isInTrigger = true;
                Trigger();
        }
        gameObject.SetActive(false);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isInTrigger = false;
    }

}
