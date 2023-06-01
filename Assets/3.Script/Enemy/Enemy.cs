using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D rb;
    protected Transform player;
    protected Animator anim;

    [Header("EnemyProperty")]
    public int level;
    public float hp;
    public float[] maxHP;
    public float[] attackPower;
    public float[] defense;
    public bool isBoss;

    [Header("Patrol")]
    private float timer = 0;
    protected int turnCount = 1;
    public float patrolSpeed;
    public float patrolMinTime;
    public float patrolMaxTime;
    public float idleMinTime;
    public float idleMaxTime;

    [Header("Warn")]
    public bool isFind;
    public float warn_X;
    public float warn_Y;
    public float warnout_X;
    public float warnout_Y;
    public int layerMask;

    [Header("Attack")]
    protected bool isAttack;
    protected float attackTimer;
    public bool isHeavyAttack;
    public float attackCalculation;
    public float attackCoolingTime;

    [Header("Hurt")] public GameObject lightHurtEffect;
    public GameObject HeavyHurtEffect;
    public float lightHurtSpeed;
    public float heavyHurtSpeed;
    protected bool isLightHurt;
    protected bool isHeavyHurt;
    protected bool isCloseHurt;
    private AnimatorStateInfo info;
    private Vector2 hurtDirection;  
    private bool isDead;
    private bool isLeaveMoney;
    public GameObject moneyInWorld;
    public int deadMoneyNum;
    public int deadMoneyValue;
    public GameObject hurtArtical;
    private float hurtTimer;
    private float hurtTime=0.2f;

    [Header("UI")]
    public GameObject floatPoint;

    [Header("Check")]
    public float checkRadiu;
    public Transform groundCheck;
    public Transform frontCheck;
    public LayerMask Ground;

    [Header("RunToPlayer")]
    
    public float closeDistance;
    public float closeSpeed;
    protected bool isFar;

    [Header("��Ч")]

    public AudioSource audioSource;
    public AudioClip[] ememySound;
    [Header("�����")]
    public float shakeTime;

    public int heavyPause;

    public float heavyStrength;

    [Header("Defense")]
    public bool isDefense;
    protected bool DefenseAttack;


    [Header("Material")] 
    public float NormalIntensity = 0.5f;
    public float flashIntensity = 100f;
    public float flashTime = 0.5f;
    private Material material;
    
    private bool isDissolving = false;

    private float fade = 1f;
    [SerializeField]protected PlayerController playerController;
    
    
    // Start is called before the first frame update
    


    protected virtual void  Start()
    {
        playerController = FindObjectOfType<PlayerController>();;
        material = GetComponent<SpriteRenderer>().material;
        moneyInWorld.GetComponent<GoldenCoin>().value = deadMoneyValue;
        layerMask = (1 << 7) | (1 << 8);
        layerMask = ~layerMask;
        player = playerController.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Dissolve();
        hurtTimer += Time.deltaTime;
        HPControl();
    }
    // Update is called once per frame
    protected void Attack()
    {
        if (isFind &&!anim.GetBool("Attacking"))
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCoolingTime)
            {
                AudioAttack01();
                if (transform.position.x < player.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);

                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                anim.SetBool("Attacking", true);
            }

        }
    }
    protected void Attack_2()
    {
        if(!isFind)
        {
            anim.SetBool("Closing", false);
        }
        if (isFind && !anim.GetBool("Attacking"))
        {
            attackTimer += Time.deltaTime;

            
            if (transform.position.x < player.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (attackTimer >= attackCoolingTime)
            {
                Invoke("AudioAttack01", 0.5f);
                anim.SetBool("Attacking", true);

            }
            else
            {
                if (isFar)
                {
                    anim.SetBool("Closing", true);
                    rb.velocity = new Vector2(-transform.localScale.x * closeSpeed, rb.velocity.y);
                }
                else
                {
                    if (anim.GetBool("Closing"))
                        attackTimer += attackCoolingTime-0.5f;
                    anim.SetBool("Closing", false);
                }
            }

        }
    }
    protected void AudioHurt()
    {
        audioSource.clip = ememySound[0];
        audioSource.Play();
    }

    public void AudioAttack01()
    {
        audioSource.clip = ememySound[1];
        audioSource.Play();
    }
    private void HPControl()
    {
        hp = Mathf.Clamp(hp, 0, maxHP[level - 1]);
        if (hp == 0&&!isLeaveMoney)
        {
            anim.SetBool("Die", true);
            isDissolving = true;
            isDead = true;
          //  Money(deadMoneyNum);
         //   isLeaveMoney = true;
            playerController.isSpeedLimit = true;
        }
    }
    public void CloseFloatPoint()
    {
        gameObject.SetActive(false);
    }
    protected void FloatPoint(float damage)
    {
        GameObject fp = Instantiate(floatPoint,transform.position,transform.rotation);
        fp.GetComponentInChildren<Text>().text =damage.ToString();
        Destroy(fp, 2f);
    }
    public void IsHeavyAttack()
    {
        isHeavyAttack = true;
    }
    public void IsLightAttack()
    {
        isHeavyAttack = false;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    protected void PlayerCloseSlow()
    {
        if (Vector3.Distance(transform.position, player.position) > 20)
        {
            playerController.isSpeedLimit = false;
           
        }
        else
        {
            playerController.isSpeedLimit = true;
        }
    }//�ж��������,ִ�м���
    public void AttackHorizentalMove(float attackMoveHorizentalSpeed)
    {
        rb.velocity = new Vector2(-transform.localScale.x * attackMoveHorizentalSpeed,rb.velocity.y);
    }//�����ƶ��ٶ�
    public void AttackVerticalMove(float attackMoveVerticalSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, attackMoveVerticalSpeed);
    }//�����ƶ��ٶ�
    protected void Patrol()
    {

        if (anim.GetBool("Idling"))
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {

                anim.SetBool("Idling", false);
                anim.SetBool("Running", true);

                if (turnCount == 0)
                {
                    this.transform.localScale = new Vector3(-this.transform.localScale.x, 1, 1);
                    turnCount++;
                }
                else
                    turnCount--;
                timer = Random.Range(patrolMinTime, patrolMaxTime);
            }
        }//��ʼѲ��


        if (anim.GetBool("Running"))
        {
            rb.velocity = new Vector2(-this.transform.localScale.x * patrolSpeed, 0);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                anim.SetBool("Running", false);
                anim.SetBool("Idling", true);


                timer = Random.Range(idleMinTime, idleMaxTime);
            }
        }//��ʼ����
    }//½��Ѳ��ai
    protected void IsFar ()
    {
        if (Mathf.Abs(player.position.x - this.transform.position.x) > closeDistance)
        {
            isFar = true;
        }
        else
        {
            isFar = false;
        }
    }
    protected virtual void IsFind( )
    {
        Debug.DrawRay(transform.position, transform.localScale.x * Vector3.left*warn_X, Color.red);
        if (!isFind&&Physics2D.Raycast(transform.position, transform.localScale.x*Vector3.left, warn_X, LayerMask.GetMask("Player")))
        {
            anim.SetBool("Running", false);
            anim.SetBool("Idling", false);
            
           // timer = attackCoolingTime;
            isFind = true;
        }
        if (!anim.GetBool("Attacking") && isFind && (Mathf.Abs(player.position.y - this.transform.position.y) > warnout_Y || Mathf.Abs(player.position.x - this.transform.position.x) > warnout_X))
        {
            anim.SetBool("Idling", true);
            isFind = false;
        }
    }//�������
    protected void Check()
    {

        if (rb.velocity.y == 0 && !Physics2D.OverlapCircle(groundCheck.position, checkRadiu, Ground))
        {
            if (!isFind)
            {
                this.transform.localScale = new Vector3(-this.transform.localScale.x, 1, 1);
                turnCount += 2;
            }

            else
            {
                rb.velocity = Vector2.zero;
            }
        }


        if (Physics2D.OverlapCircle(frontCheck.position, checkRadiu, Ground))
        {
            if (!isFind)
            {

                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                turnCount += 2;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }


        }


    } //����Ƿ�ײǽ���ߵ���
    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }//��ֹ�����ã������¼�
    public void AttackOver()
    {
        anim.SetBool("Attacking", false); 
        anim.SetBool("Idling", true);
        rb.velocity = Vector2.zero;
        attackTimer = 0;
        isAttack = false;
    }//�����¼������������¼�
    protected void Hurt()
    {
       // isFind = true;

        info = anim.GetCurrentAnimatorStateInfo(0);
        if (isLightHurt)
        {
            rb.velocity = hurtDirection * lightHurtSpeed;
            isLightHurt = false;
        }
        if (isHeavyHurt)
        {
            rb.velocity = hurtDirection * heavyHurtSpeed;
            if (info.normalizedTime >= 0.8f)
                isHeavyHurt = false;
        }
    }

    private IEnumerator HurtFlash()
    
    {
        material.SetFloat("_Damaged", flashIntensity);
        yield return new WaitForSeconds(flashTime);
        material.SetFloat("_Damaged",NormalIntensity);
    }
    
    public void GetHit(Vector2 hurtDirection,int hurtType)
    {
        Debug.Log("已受伤");
        FloatPoint(playerController.attackCalculation);
        hp -= playerController.attackCalculation;
        AudioHurt();
        StartCoroutine(HurtFlash());
        Instantiate(hurtArtical, transform.position,transform.rotation,transform);
        if (hurtType == 1)
        {
            isLightHurt = true;
            lightHurtEffect.SetActive(true);
            if(!isBoss)
            anim.SetTrigger("LightHurting");
        }
        else
        {
            isHeavyHurt = true;
            HeavyHurtEffect.SetActive(true);
            if(!isBoss)
            anim.SetTrigger("HeavyHurting");
            playerController.IsShake();
        }
        transform.localScale = new Vector3(hurtDirection.x, 1, 1);
        hurtArtical.transform.localScale = new Vector3(hurtDirection.x, 1, 1);
            isLightHurt = true;
            this.hurtDirection = hurtDirection;
    }

    public void AttackCalculation(float magnification)
    {
       
        attackCalculation = attackPower[level-1] * magnification;
       
    }
    public void IsAttack(int thisAttack)
    {
        if(thisAttack>0)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
    }

    private void Money(int num)
    {
        for(int i = 1; i <= num; i++)
        {
            Instantiate(moneyInWorld,transform.position,transform.rotation);
        }
    }
    private void Dissolve()
    {

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade<=0f)
            {
                fade = 0f;
                isDissolving = false;
            }
            material.SetFloat("_Fade",fade);
        }
    }


}
    