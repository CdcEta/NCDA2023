using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate_X;
    public float moveRate_Y;
    private float startPointX;
    private float startPointY;
    void Start()
    {

        startPointX = transform.position.x;    
        startPointY = transform.position.y;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = new Vector2(startPointX + cam.position.x * moveRate_X, startPointY + cam.position.y * moveRate_Y);
    }
}
