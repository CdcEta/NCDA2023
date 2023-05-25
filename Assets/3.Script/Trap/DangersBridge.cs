using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangersBridge : MonoBehaviour
{
    private PlatformEffector2D platform;
    public Sprite closeSprite;
    public Sprite openSprite;
    private SpriteRenderer spriteRenderer;
    public void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        platform = GetComponent<PlatformEffector2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine("StayOn");
        }
    }

    IEnumerator StayOn()
    {
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = openSprite;
        platform.rotationalOffset = 180;
        yield return new WaitForSeconds(4f);
        spriteRenderer.sprite = closeSprite;
        platform.rotationalOffset = 0;
        yield return null;
    }


}
