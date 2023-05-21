using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PlanePause;
    private int f=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            f = f + 1;
            if(f%2==1)
            {
                PlanePause.SetActive(true);
                Time.timeScale = 0;
            }
            if(f%2==0)
            {
                PlanePause.SetActive(false);
                Time.timeScale = 1;
            }

        }
    }
}
