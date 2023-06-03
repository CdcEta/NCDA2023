using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RecordPoint : MonoBehaviour
{
    private Animator anim;
    private bool isTouch;
    public bool isFire;
    private bool isRecord;
    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.F)&&isTouch)
        {
            anim.SetBool("Recording", true);
            if (!isRecord)
            {
                if (isFire)
                {
                    SoundManager.instance.FireRecord();
                }
                else
                {
                    SoundManager.instance.LightningRecord();
                }
                isRecord = true;
                PlayerController.respawnPoint = transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.CompareTag("Player"))
        {
            isTouch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouch = false;
        }
    }
}
