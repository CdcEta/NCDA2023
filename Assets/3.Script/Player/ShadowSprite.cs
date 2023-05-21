using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("时间控制参数")]
    public float activeTime;
    public float activeStart;
    



    [Header("不透明参数")]
    private float alpha;
    public float alphaSet;
    public float alphaMultipler;


    // Update is called once per frameha

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;


    }

    void Update()
    {
        alpha = alphaMultipler*alpha;

        color = new Color(1, 1, 0, alpha);

        thisSprite.color = color;

        if(Time.time>= activeStart+activeTime)
        {
            //return Object Pool;
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
           

          
    }
}
