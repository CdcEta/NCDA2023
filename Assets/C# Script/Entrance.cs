using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
     public Vector3 goToPos;

    private Transform playerPos;
    public int nextSceneID;
    private GameObject stayScene;

    

    private bool isInDoor;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nextSceneID == 8)
            ButtonManagerMenu.startGame = false;
        GoToRoom();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInDoor = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInDoor = false;
    }

    private void GoToRoom()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInDoor)
        {

            stayScene = GameObject.Find("StaticObject");
            DontDestroyOnLoad(stayScene);
            playerPos.transform.position = goToPos;
            SceneManager.LoadScene(nextSceneID);
        }
    }
}
