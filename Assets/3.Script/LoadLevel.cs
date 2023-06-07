using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public string nextScene;

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }

    IEnumerator Load(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadNextLevel(nextScene);
    }
}
