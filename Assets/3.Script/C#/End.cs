using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Back", 35f);
    }

    void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}

