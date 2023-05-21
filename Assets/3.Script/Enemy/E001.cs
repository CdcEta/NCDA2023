using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E001 : Enemy

{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        IsFind();
        Attack();
        PlayerCloseSlow();
        Hurt();
        Patrol();
        Check();
     
    }



    public void AudioAttack02()
    {
        audioSource.clip = ememySound[2];
        audioSource.Play();
    }
    protected override void IsFind()
    {
        Debug.DrawRay(transform.position, transform.localScale.x * Vector3.left * warn_X, Color.red);
        if (!isFind && Physics2D.Raycast(transform.position, transform.localScale.x * Vector3.left, warn_X, LayerMask.GetMask("Player")))
        {
            anim.SetBool("Idling", false);
            anim.SetBool("Running", false);
                anim.SetTrigger("Warning");
            // timer = attackCoolingTime;
            isFind = true;
        }
        if (!anim.GetBool("Attacking") && isFind && (Mathf.Abs(player.position.y - this.transform.position.y) > warnout_Y || Mathf.Abs(player.position.x - this.transform.position.x) > warnout_X))
        {
            anim.SetBool("Idling", true);
            isFind = false;
        }
    }//·¢ÏÖÍæ¼Ò



}
