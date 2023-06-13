using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Boss01 : Enemy
{

    [Header("Attack")] public SpriteRenderer spriteRenderer;
    public PlayableDirector playableDirector;
    public float attack01MoveBackSpeed;
    public float attack01MoveJumpSpeed;
    public float attack01MoveForwardSpeed;

    public float defenseCoolingTime;
    public float defenseTimer;
    public float attack2Timer;
    public float attack2CoolingTime;

    public float defenseSpeed;
    public bool attackMode2;

    public Image bossHPBar;
    public GameObject sword;
    private Vector3 posi;
    public GameObject parentObject;
    public GameObject talkBoss;
    public GameObject loseBoss;
    private int skillCount;
    private bool isSkill;
    private void Awake()
    {
        
        talkBoss.SetActive(true);
        posi = gameObject.transform.position;
        gameObject.transform.position =talkBoss.transform.position;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(BossAppear(spriteRenderer));
        bossHPBar.fillAmount = hp / maxHP[level - 1];
        transform.position = posi;
    }
    protected override void Start()
    {
        
        base.Start();
    }
    private IEnumerator BossAppear(SpriteRenderer t)
    {
        while (t.color.a < 1)
        {
            t.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (hp == 0)
        {
            rb.velocity = Vector2.zero;
            playableDirector.Play();
            AudioAtop();
            //    playerController.ChooseEnd();
        }
      
        if (hp != 0)
        {
            base.Update();
            Boss01Attack();
            IsFar();
            Direction();
            Boss01Attack2();
            Hurt(); 
        }
   
        bossHPBar.fillAmount = hp / maxHP[level - 1];

        if (playerController.isDead)
        {
            //   hp = maxHP[level - 1];
            // bossHPBar.fillAmount = hp / maxHP[level - 1];

        }
        if ((hp < maxHP[0] / 3 * 2 && skillCount == 0) || (hp < maxHP[0] / 3 && skillCount == 1))
        {
            isSkill = true;
            skillCount++;
        }
    }


    public void AudioAttack02()
    {
        audioSource.clip = ememySound[2];
        audioSource.Play();
    }
    public void AudioAtop()
    {
        audioSource.Pause();
    }
    public void AudioAttack03()
    {
        audioSource.clip = ememySound[3];
        audioSource.Play();
    }
    public void AudioAttack04()
    {
        Invoke("SwordWave", 1f);
    }

    private void SwordWave()
    {
        audioSource.clip = ememySound[4];
        audioSource.Play();
    }
    protected void Boss01Attack()
    {
     
        if (!attackMode2&&!isAttack)
        {
            attackTimer += Time.deltaTime;


            if (attackTimer >= attackCoolingTime)
            {
                if (isFar)
                {
                    anim.SetBool("Idling", false);
                    anim.SetBool("Running", true);
                    rb.velocity = new Vector2(-transform.localScale.x * closeSpeed, rb.velocity.y);

                }
                else
                {
                    rb.velocity = Vector2.zero;
                    if (anim.GetBool("Running"))
                    {
                        anim.SetBool("Running", false);
                        anim.SetBool("Attacking", true);
                    }
                  
                    anim.SetInteger("AttackType", 4);
                    defenseTimer = 0;
                    attackMode2 = true;
                }
            }
        }
    }

    protected void Direction()
    {
        if (anim.GetBool("Attacking")==false)
        {
            if (transform.position.x < player.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    protected void Boss01Attack2()
    {
        if (attackMode2 && !isAttack)
        {
            defenseTimer += Time.deltaTime;
            
            if (defenseTimer >= defenseCoolingTime)
            {
                anim.SetBool("Defensing", true);
                attack2Timer += Time.deltaTime;
                rb.velocity = new Vector2(transform.localScale.x * defenseSpeed, rb.velocity.y);
                if (attack2Timer >= attack2CoolingTime && !DefenseAttack)
                {
                    isAttack = true;
                    anim.SetBool("Attacking", true);
                    anim.SetBool("Defensing", false);
                    isDefense = false;
                    defenseTimer = 0;
                    attack2Timer = 0;
                    if (isSkill)
                    {
                        anim.SetInteger("AttackType", 5);
                        isDefense = true;
                        attackMode2 = false;
                        isSkill = false;
                    }
                    else if (Mathf.Abs(player.position.x - this.transform.position.x) >= 120 && Mathf.Abs(player.position.x - this.transform.position.x) < 360)
                    {
                        anim.SetInteger("AttackType", 1);
                        isDefense = false;
                        attackMode2 = false;
                    }
                    else if (Mathf.Abs(player.position.x - this.transform.position.x) < 120)
                    {
                        anim.SetInteger("AttackType", 2);
                        isDefense = false;
                        attackMode2 = false;
                    }
                    else if (Mathf.Abs(player.position.x - this.transform.position.x) >= 360)
                    {
                        anim.SetInteger("AttackType", 5);
                        isDefense = true;
                        attackMode2 = false;
                    }
                }
            }

        }
    }

    public void SwordStart(float rotateZ)
    {
        sword.transform.position = player.transform.position;
        Instantiate(sword);
        sword.transform.Rotate(0, 0, 72);

    }



}
