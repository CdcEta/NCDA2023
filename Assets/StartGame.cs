using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject playerStart;
    // Start is called before the first frame update
    void Start()
    {
        if (ButtonManagerMenu.startGame)
        {
            playerStart.SetActive(true);
            ButtonManagerMenu.startGame = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
