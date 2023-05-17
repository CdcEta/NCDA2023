using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Animator anim;
    private bool isFly=false;
    public float horizontalSpeed;
    public float verticalSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Flying");
            isFly = true;
        Destroy(gameObject, 5f);
    }

    private void Start()
    {
        anim= GetComponent<Animator>();
    }
    private void Update()
    {
        if(isFly)
        transform.Translate(horizontalSpeed * gameObject.transform.localScale.x*Time.deltaTime*-1, verticalSpeed*Time.deltaTime, 0f);
    }
}
