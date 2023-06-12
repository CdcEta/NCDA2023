using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public bool isLoadNext;
    public Animator transition;
    public float transitionTime = 1f;
    public string nextScene;
    private PlayerController playerController;
    public PlayerProperty property;
        // Update is called once per frame
    void Update()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController.isDead)
        {
            StartCoroutine(LoadNow());
        }

        if (isLoadNext)
        {
            LoadNextLevel(nextScene);
        }
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

    IEnumerator LoadNow()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            property.hp = playerController.hp;
            property.maxHP = playerController.maxHP;
            property.energy =playerController.energy;
            property.getAxe =playerController.GetAxe;
            property.getSword =playerController.GetSword;
            LoadNextLevel(nextScene);
        }

    }
    
}
