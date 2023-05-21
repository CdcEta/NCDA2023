using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource[] audioSource;
    public AudioClip[] sound;
    public AudioClip[] backgroundMusic;

    private void Awake()
    {
        instance = this;
    }
   
    public void Sword01()
    {
        audioSource[0].clip = sound[0];
        audioSource[0].Play();
    }
    public void Sword02()
    {
        audioSource[0].clip = sound[1];
        audioSource[0].Play();
    }

    public void DownAttack()
    {
        audioSource[0].clip = sound[2];
        audioSource[0].Play();
    }

    public void Hurt()
    {
        audioSource[0].clip = sound[3];
        audioSource[0].Play();
    }

    public void Run()
    {
        if (audioSource[1].clip != sound[4])

        {
            audioSource[1].clip = sound[4];

            audioSource[1].Play();
        }

        if(!audioSource[1].isPlaying)
        {
            audioSource[1].Play();
        }
    }
    public void RunStop()
    {
        
        audioSource[1].Stop(); 

    }
    public void Jump()
    {
        audioSource[0].clip = sound[5];
        audioSource[0].Play();
    }

    public void Shoot()
    {
        audioSource[0].clip = sound[6];
        audioSource[0].Play();
    }

    public void Axe01()
    {
        audioSource[0].clip = sound[7];
        audioSource[0].Play();
    }
    public void Axe02()
    {
        audioSource[0].clip = sound[8];
        audioSource[0].Play();
    }
    public void Lance01()
    {
        audioSource[0].clip = sound[9];
        audioSource[0].Play();
    }

    public void Lance02()
    {
        audioSource[0].clip = sound[10];
        audioSource[0].Play();
    }

    public void Coin()
    {
        audioSource[0].clip = sound[11];
        audioSource[0].Play();
    }

    public void Buy()
    {
        audioSource[0].clip = sound[12];
        audioSource[0].Play();
    }


    public void Roll()
    {
        audioSource[0].clip = sound[13];
        audioSource[0].Play();
    }

    public void LightningRecord()
    {
        audioSource[0].clip = sound[14];
        audioSource[0].Play();
    }    
    public void FireRecord()
    {
        audioSource[0].clip = sound[15];
        audioSource[0].Play();
    }    
    public void Opendoor()
    {
        audioSource[0].clip = sound[16];
        audioSource[0].Play();
    }   
    public void Change()
    {
        audioSource[2].clip = sound[17];
        audioSource[2].Play();
    }
}
