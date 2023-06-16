using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    private BloomControl bloomControl;
    public bool menuLoader;
    public bool isLoadNext;
    public Animator transition;
    public float transitionTime = 1f;
    public string nextScene;
    public bool noPlayer;
    public PlayerProperty property;
    public float beginTime;
        // Update is called once per frame
        private void Start()
        {
        }

        void Update()
    {
        if (menuLoader)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LoadNextLevel(nextScene);
            }
        }
        if(!noPlayer){
        PlayerController playerController = FindObjectOfType<PlayerController>();
        
        if (playerController.isDead)
        {
            StartCoroutine(LoadNow());
        }
}
        if (isLoadNext)
        {
            LoadNextLevel(nextScene);
        }
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(Load(sceneName,beginTime));
    }

    IEnumerator Load(string sceneName,float beginTime)
    {
        yield return new WaitForSeconds(beginTime);
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
            PlayerController playerController = FindObjectOfType<PlayerController>();
            property.hp = playerController.hp;
            property.maxHP = playerController.maxHP;
            property.energy =playerController.energy;
            property.getAxe =playerController.GetAxe;
            property.getSword =playerController.GetSword;
            LoadNextLevel(nextScene);
        }

    }
    
}
