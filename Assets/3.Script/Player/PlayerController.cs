using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //单例模式
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if (instance = null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            else
            {
                Debug.Log("未找到玩家实例");
            }
            return instance;
        }
    }

    
    public GameObject cinemaCollider;
    
    [Header("Other")]
    private Rigidbody2D rb;
    public LayerMask Ground;
    public LayerMask Platform;
    [Header("PlayerProperty")]
    public float hp;
    public float maxHP;
    [Header("Animator")]
    private Animator anim;
    public AnimationClip[] moveAnimationClip;
    public AnimatorOverrideController animatorMoveOverrideController;
    
    [Header("Inventory")]
    public Inventory playerInventory;
    [SerializeField] private bool GetSword;
    [SerializeField] private bool GetAxe;
    public int id;
    public int comboStep;
    public float interval = 2f;
    private float timer;
  
    public float lightspeed;
    public float playerHurt;
    public float attackCalculation;
 

    [Header("Attack")]
    [SerializeField]private int attackType;
    public bool isAttack;
    public float shakeTime;
    public float lightPause;
    public float lightStrength;
    public float heavyPause;
    public float heavyStrength;
    

    [Header("Dash")]
    [SerializeField] private float dashingPower = 20f;
    [SerializeField]private float dashingTime = 0.2f;
    [SerializeField] private float dashCoolingTime = 2f;
    private bool canDash = true;
    private bool isDashing;

    
    
    
    [Header("Move")]

    public float runSpeed;
    public float walkSpeed;
    public float runTime;
    private float runTimer;
    public float speed;
    private bool isRun;
    float horizontalmove;
    private bool isGetUp, isRoll, isGround;


    public float airSpeed;
    public float maxfalltime = 1f;
    private float rollTimer;
    private Vector3 targetPosition;

    [Header("Jump")]
    public float jumpforce;
    public int JumpCount;
    public float getUpSpeed;

    [Header("Ground")] private bool isPlatform;
    public Transform groundCheck;
    public float checkRadiu;
    public Transform frontCheck;

    private bool isOnCorner;



    [Header("DownAttack")]
    public float downAttackInterval = 0.5f;
    public float downAttackSpeed;
    public float downTimer;
    public int downNumber;
    public float[] downAttackPower;
    public GameObject downAttackPaticle;

    public float downAttackTime;
    private float downAttackTimer;

    [Header("ClimbLatter")]
    public float gameGravity;
    private bool isClimbLadder;

    [Header("Defense")]

    public bool isDefense;

    [Header("SpeedLimit")]

    public bool isSpeedLimit;

    [Header("Hurt")] private bool isBack;
    public float lightHurtSpeed;
    public float heavyHurtSpeed;
    public bool isDead;
    public Animator RedShine;

    [Header("��Ļ����")]
    public Cinemachine.CinemachineImpulseSource MyInpulse;
    
    [Header("UI")]
    public GameObject floatPoint;
    public Image hpBar;
    public Text hpText;
    public GameObject moneyPoint;
    private int state;

    [Header("Transport")] public Vector3 backPoint;
    private float backTimer;
    public float backCountTime=10f;
    public static Vector3 respawnPoint;
    [Header("DrinkDrug")]
    public static int DrinkCount=3;
    private bool isDrinking;
    [Header("Recovery")]
    public Text RecoveryCount;

    public GameObject[] stage;
    
    
    [Header("Material")] 
    public float NormalIntensity = 0.5f;
    public float flashIntensity = 100f;
    public float flashTime = 0.5f;
    private Material material;
    private bool isDissolving = false;
    private float fade = 1f;
    void Start()
    {
        respawnPoint = backPoint;
        material = GetComponent<SpriteRenderer>().material;
        // playerInventory.itemList[0] = default;
        MyInpulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isRun = true;
    }
    // Update is called once per frame
    private void Update()
    {
        BackPlaceUpdate(backCountTime);
        FindBound();
        animatorMoveOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorMoveOverrideController;

        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadiu, Ground |Platform);
        isPlatform = Physics2D.OverlapCircle(groundCheck.position, checkRadiu, Platform);
        //  isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadiu,Platform ) ;
     //   isTouchingFront = (Physics2D.OverlapCircle(frontCheck.position, checkRadiu, Ground));
        IsRun();
        WeaponSwitch();
        Jump();
        Roll();
        DownAttack();
        HPControl();
        Drink();    
        RecoveryCount.text = DrinkCount.ToString();
        

    }
    
    
    public void FindBound()
    {
        if(GameObject.Find("Bound"))
            cinemaCollider.GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find("Bound").GetComponent<PolygonCollider2D>();
    }
    void FixedUpdate()
    {
        Movement();
    }
    public void IsShake()
    {
        MyInpulse.GenerateImpulse();
    }
    
    private void DownAttack()
    {
        downAttackTimer+= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.S) && anim.GetBool("Falling")&&downAttackTimer>=downAttackTime&&JumpCount==0)
        {
            anim.SetBool("DownAttacking", true);
            rb.velocity = new Vector2(0, -downAttackSpeed);
            rb.constraints = (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX);
            downAttackTimer = 0;
        }
        if ((rb.velocity == Vector2.zero || isGround) && anim.GetBool("DownAttacking"))
        {
            IsShake();
            //SoundManager.instance.DownAttack();
            anim.SetBool("DownAttacking", false);
            Invoke("DownAttackGetUp", 0.6f);
            Instantiate(downAttackPaticle,transform.position,transform.rotation);
        }
    }
    private void DownAttackGetUp()
    {

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void Movement()
    {
        isGetUp = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerGetUp");
        horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        //��ɫ�ƶ�
        floatPoint.transform.localScale = new Vector3(gameObject.transform.localScale.x,1,1);
        if (!isDrinking&&!isDead && !isDefense && !isAttack && !isGetUp && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerHeavyHurt") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !anim.GetBool("DownAttacking") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDownAttackGetUp") && !isOnCorner)
        {
            anim.SetFloat("Running", Mathf.Abs(facedirection));
            runTimer += Time.deltaTime;
            if (isGround&&facedirection!=0)
            {
                // if (!isSpeedLimit)
                rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);
                //  else
                // {
                //      rb.velocity = new Vector2(horizontalmove * speed*.3f, rb.velocity.y);
                //  }


            }
            else
            {
                rb.velocity = new Vector2(horizontalmove * airSpeed, rb.velocity.y);
            }
        }
        else
        {
            runTimer = 0;
            anim.SetFloat("Running", 0);
        }

        if (isRun && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRun") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerHeavyHurt"))
        {
            ////SoundManager.instance.Run();
        }
        else
        {
           // //SoundManager.instance.RunStop();
        }
        
        if (runTimer >= runTime && isRun)
        {
            anim.SetBool("RunOver", true);
        }
        else
        {
            anim.SetBool("RunOver", false);
        }

        //��ɫ����
        if (!isOnCorner && facedirection != 0 && !isAttack)
        {
            transform.localScale = new Vector3(facedirection * -1, 1, 1);
        }
    }
    private void IsRun()
    {
        if (isRun)
        {
            speed = runSpeed;
            animatorMoveOverrideController["PlayerRun"] = moveAnimationClip[1];
            // if (Input.GetKeyDown(KeyCode.LeftControl))
            //{

            //  isRun = false;

            //}
        }
        else
        {
            speed = walkSpeed;
            animatorMoveOverrideController["PlayerRun"] = moveAnimationClip[0];
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {

                isRun = true;

            }
        }
    }
    
    private void Roll()
    {
       
        rollTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && rollTimer > dashCoolingTime&&!isAttack&&!isOnCorner)
        {
            isRoll = true;
            rb.gravityScale = 0;
            anim.SetTrigger("Rolling");
            //SoundManager.instance.Roll();
            rollTimer = 0;
        }
        
        if (isRoll&&!isOnCorner)
        {
            rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        }
    }
    private void Jump()
    {
        if (rb.velocity.y < -10)
        {

            anim.SetBool("Falling", true);
            anim.SetBool("Jumping", false);
        }
        if (rb.velocity.y >= -10 && rb.velocity.y <= 0)
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }
        if (rb.velocity.y > 0)
        {

            anim.SetBool("Jumping", true);
            anim.SetBool("Falling", false);
        }
        if (Input.GetButtonDown("Jump") && isGround && !isAttack && !isRoll && !isOnCorner && !isDead && !isGetUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            //SoundManager.instance.Jump();
        }
        if (Input.GetButtonDown("Jump") && !isGround && JumpCount > 0 && !isAttack && !isOnCorner)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            anim.Play("PlayerJump", 0, 0f);
            JumpCount -= 1;
            //SoundManager.instance.Jump();
        }
        anim.SetBool("GetUp", false);
        if (isGround)
        {
            JumpCount = 1;
            anim.SetBool("Falling", false);
            //  anim.SetBool("Jumping", false);
            if (rb.velocity.y < -getUpSpeed)
                anim.SetBool("GetUp", true);
            else
                anim.SetTrigger("Idling");

        }
    }
    public void IdleSpeed()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    public void RollOver()
    {
        isRoll = false;
        if(!isOnCorner)
            rb.gravityScale = gameGravity;
    }
    public void CornerMove()
    {
        isOnCorner = false;
        rb.velocity = Vector2.zero;
        this.transform.position = new Vector3(this.transform.position.x - this.transform.localScale.x * 38.8f, this.transform.position.y + 28.4f, this.transform.position.z);
         rb.gravityScale = gameGravity;



    }
    public void AttackOver()
    {
        isAttack = false;
        rb.gravityScale = gameGravity;
    }
    private void GetHeavyHit(float hurtDirection, float hurtCalculation)
    {
        //isLghtHurt = true;
        //this.hurtDirection = hurtDirection;
        if (!isRoll && !anim.GetBool("DownAttacking") && !isDead)
        {
            rb.velocity = new Vector3(-hurtDirection * heavyHurtSpeed, 0, 0);
            hp -= hurtCalculation;
            anim.SetTrigger("HeavyHurting");
            AttackOver();
            FloatPoint(hurtCalculation);
            RedShine.SetTrigger("Shining");
            //SoundManager.instance.Hurt();
        }
    }
    private void GetLightHit(float hurtDirection, float hurtCalculation)
    {
        //isLghtHurt = true;
        //this.hurtDirection = hurtDirection;
        if (!isRoll && !anim.GetBool("DownAttacking") && !isDead)
        {
            rb.velocity = new Vector3(-hurtDirection * lightHurtSpeed, 0, 0);
            hp -= hurtCalculation;
            anim.SetTrigger("LightHurting");
            FloatPoint(hurtCalculation);
            AttackOver();
            RedShine.SetTrigger("Shining");
            //SoundManager.instance.Hurt();
        }
    }

    private void HPControl()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        if (hp == 0)
        {

            anim.SetTrigger("Die");
            isDead = true;
        }
        hpBar.fillAmount = hp / maxHP;
        hpText.text = hp + "/" + maxHP;
    }
    public void Respawn()
    {
        anim.SetBool("Die", false);
        if (isDead)
        {
            hp = maxHP;
            transform.position = respawnPoint;
            isDead = false;
        }

        if (isBack)
        {
            hp -= 1;
            transform.position = backPoint;
            isBack = false; 
        }
    }

    private void BackPlaceUpdate(float backCountTime)
    {
        backTimer += Time.deltaTime;
        if (backTimer > backCountTime&& isGround&&!isPlatform)
        {
            backPoint = transform.position;
            backTimer = 0;
        }
    }
    private void AttackCalculation(int stage)
    {
       
        attackCalculation = playerInventory.itemList[0].WeaponAttackPower;
    }
    private void FloatPoint(float damage)
    {
        floatPoint.SetActive(false);
        floatPoint.GetComponent<Text>().text = damage.ToString();
        floatPoint.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathTrap"))
        {
            StartCoroutine(Rebirth());
            anim.SetTrigger("Die");
            isBack = true;
        }
        if (collision.CompareTag("EnemyBox"))
        {
            Debug.Log("已攻击到敌人");
            if (attackType == 1)
            {
                AttackSense.Instance.HitPause(lightPause);
                AttackSense.Instance.CameraShake(shakeTime,lightStrength);
            }
            else if (attackType == 2)
            {
                AttackSense.Instance.HitPause(heavyPause);
                AttackSense.Instance.CameraShake(shakeTime,heavyStrength);
            }

            if (transform.localScale.x > 0)
            {
                collision.GetComponentInParent<Enemy>().GetHit(Vector2.left, attackType);
            }
            else if (transform.localScale.x < 0)
            {
                collision.GetComponentInParent<Enemy>().GetHit(Vector2.right, attackType);
            }
        }
        if (collision.CompareTag("Corner") && !isRoll&&this.transform.localScale.x * collision.transform.localScale.x < 0 && !anim.GetBool("DownAttacking") && !isAttack)
        {

            rb.velocity = Vector2.zero;
            isOnCorner = true;
            this.transform.position = new Vector3(collision.transform.position.x - 5f, collision.transform.position.y + 25f, this.transform.position.z);
            anim.SetTrigger("OnCorner");
            rb.gravityScale = 0;
            //  Invoke("CornerMove", 0.45f);
        }
        if (collision.CompareTag("EnemyArrow"))
        {
            GetLightHit(collision.transform.parent.localScale.x, collision.GetComponentInParent<EnemyArrow>().attackPower);
        }
        if (collision.CompareTag("EnemyTrigger"))
        {
            if (!collision.GetComponentInParent<Enemy>().isHeavyAttack)
            {
                
                GetLightHit(collision.transform.parent.localScale.x, collision.GetComponentInParent<Enemy>().attackCalculation);
                
            }
            else
            {
                GetHeavyHit(collision.transform.parent.localScale.x, collision.GetComponentInParent<Enemy>().attackCalculation);

            }
        }
        if (collision.CompareTag("Skill"))
        {
            if (!isRoll && !anim.GetBool("DownAttacking") && !isDead)
            {
                hp -= 20;
                anim.SetTrigger("HeavyHurting");
                AttackOver();
                FloatPoint(20);
                //SoundManager.instance.Hurt();
            }
        }
    }
    public void AttackHorizentalMove(float attackMoveHorizentalSpeed)
    {
        rb.velocity = new Vector2(-transform.localScale.x * attackMoveHorizentalSpeed, rb.velocity.y);
    }
    public void AttackVerticalMove(float attackMoveVerticalSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, attackMoveVerticalSpeed);
    }
    private void WeaponSwitch()
    {
        if ( !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") &&
            !isAttack && !isOnCorner)
        {
            if (Input.GetMouseButtonDown(0))
            {
                id = 1;
                anim.SetInteger("AttackID", id);
                SwordControl();
            }
            if (Input.GetMouseButtonDown(1))
            {
                id = 7;
                anim.SetInteger("AttackID", id);
                AxeControl();
            }  
            
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = interval;
            comboStep = 0;
        }
    }
    private void SwordControl()
    {
        isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            AttackHorizentalMove(50);
            if (comboStep > 3)
                comboStep = 1;
            if (comboStep != 3)
            {

                attackType = 1;
                if (comboStep == 1)
                {
                    AttackCalculation(1);
                }
                else
                {
                    AttackCalculation(2);
                }
                //SoundManager.instance.Sword01();
            }
            else
            {
                attackType = 2;
                AttackCalculation(3);
                //SoundManager.instance.Sword02();
            }
            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        

    }
    // public void InstantiateArrow(float addY)
    // {
    //     arrowPosition = new Vector3(transform.position.x - transform.localScale.x * 20f, transform.position.y + addY, transform.position.z);
    //     GameObject thisArrow = Instantiate(arrow, arrowPosition, transform.rotation);
    //     thisArrow.transform.localScale = transform.localScale;
    //     thisArrow.GetComponent<SpriteRenderer>().sprite = playerInventory.itemList[0].ArrowImage;
    //     thisArrow.GetComponent<Arrow>().attackPower = playerInventory.itemList[0].WeaponAttackPower[level - 1] * playerInventory.itemList[0].magnification[0];
    // }
    private void AxeControl()
    {

            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            /*         if (comboStep != 2)
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed, rb.velocity.y);
                         //SoundManager.instance.Sword01();

                     }
                     else
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed * 2f, rb.velocity.y);
                         //SoundManager.instance.Sword02();
                     }*/


            if (comboStep > 2)
                comboStep = 1;
            if (comboStep == 1)
            {
                attackType = 1;
                AttackCalculation(1);
                AttackHorizentalMove(50);
                //SoundManager.instance.Axe01();
            }
            else
            {
                attackType = 2;
                AttackCalculation(2);
                //SoundManager.instance.Axe02();
            }
            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);

    }
    private void ConstantShoot()
    {
        //SoundManager.instance.Shoot();
    } 
    private void MoneyPoint()
    {
        moneyPoint.SetActive(false);
        moneyPoint.SetActive(true);
    }
    private void Drink()
    {
        if(Input.GetKeyDown(KeyCode.R)&&!isDrinking&&hp<maxHP)
        {
            if(DrinkCount>=1)
            {
                DrinkCount--;
                anim.SetBool("Drinking", true);
                isDrinking = true;
                Invoke("EndDrink", 1.5f);
            }
        }
    }
    private void EndDrink()
    {
        hp += maxHP / 2;
        anim.SetBool("Drinking",false);
        isDrinking = false;
    }
    


    private IEnumerator Rebirth()
    {
        while (fade > 0f)
        {
            fade -= Time.deltaTime;
            material.SetFloat("_Fade", fade);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (fade<1f)
        {
            fade += Time.deltaTime;
            material.SetFloat("_Fade", fade);
            yield return null;
        }
    }
}