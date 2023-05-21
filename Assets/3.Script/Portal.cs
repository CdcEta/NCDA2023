using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Vector3 goToPos;

    private Transform playerPos;
    public int nextSceneID;
    private GameObject stayScene;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        if (nextSceneID == 8)
            ButtonManagerMenu.startGame = false;
    }

    // Update is called once per frame
    void Update()

    {
        DontDestroy();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void DontDestroy()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stayScene = GameObject.Find("StaticObject");
            DontDestroyOnLoad(stayScene);
            playerPos.transform.position = goToPos;
            SceneManager.LoadScene(nextSceneID);
        }
    }
}
