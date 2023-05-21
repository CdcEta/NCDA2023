using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImove : MonoBehaviour
{
    bool startDrag;
    Vector2 startPos; 
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (startDrag)
        {
            transform.position = Input.mousePosition;
        }
    }
    public void StartDragUI()
    {
        startDrag = true;
    }
    public void StopDragUI()
    {
        startDrag = false;
        transform.position = startPos;
    }
}
