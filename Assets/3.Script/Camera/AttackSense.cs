using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSense : MonoBehaviour
{
    // Start is called before the first frame update
    private static AttackSense instance;
    
    public static AttackSense Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<AttackSense>();
            return instance;
        }
    }

    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
   
  
}