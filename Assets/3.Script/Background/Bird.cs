using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Animator anim;
    public bool stopMove;   
    private bool isFly=false;
    public float horizontalSpeed;
    public float verticalSpeed;
    public void Start()
    {
        anim = GetComponent<Animator>();
        if (stopMove)
        {
            anim.speed = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.speed = 1;
        anim.SetTrigger("Flying");
            isFly = true;
        Destroy(gameObject, 5f);
    }


 
    private void Update()
    {
        if(isFly)
        transform.Translate(horizontalSpeed * gameObject.transform.localScale.x*Time.deltaTime*-1, verticalSpeed*Time.deltaTime, 0f);
    }
}
