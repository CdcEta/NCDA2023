using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject tip;
    public GameObject ttip;
    protected bool isStay;
    public Animator anim;
    public bool isNPC;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tip.SetActive(true);
            isStay = true;
            anim.SetBool("Appear", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStay = false;
            anim.SetBool("Appear", false);
        }
    }

}
