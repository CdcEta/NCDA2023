using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    private PlatformEffector2D platform;

    public void Start()
    {
        platform = GetComponent<PlatformEffector2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine("StayOn");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
           StopCoroutine("StayOn");
        }
    }

    IEnumerator StayOn()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                platform.rotationalOffset = 180;
                StartCoroutine("Exit");
            }
            yield return null;
        }
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(1f);

        platform.rotationalOffset = 0;
    }
}
