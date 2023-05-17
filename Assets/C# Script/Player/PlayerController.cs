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
            return instance;
        }
    }

    
    public GameObject cinemaCollider;
    
    [Header("Other")]
    private Rigidbody2D rb;
    public LayerMask Ground;

    [Header("PlayerProperty")]
    public float hp;
    public float maxHP;
    [Header("Animator")]
    private Animator anim;
    public AnimationClip[] moveAnimationClip;
    public AnimatorOverrideController animatorMoveOverrideController;
    
    [Header("Inventory")]
    public Inventory playerInventory;

    [Header("Attack")]
    public int id;
    public int comboStep;
    public float interval = 2f;
    private float timer;
    public bool isAttack;
    public float lightspeed;
    public float playerHurt;
    public float attackCalculation;
    public bool isHeavyAttack;

    [Header("Arrow")]

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
    public float rollSpeed;
    public float rollTime;
    private float rollTimer;
    public float rollDistance;
    private Vector3 targetPosition;

    [Header("Jump")]
    public float jumpforce;
    public int JumpCount;
    public float getUpSpeed;

    [Header("Wall")]
    bool isTouchingFront;
    public LayerMask Wall;
    public Transform groundCheck;
    public float checkRadiu;
    public Transform frontCheck;
    private bool isWallSliding;
    private bool isOnCorner;

    public float wallSlidingSpeed;

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

    [Header("Hurt")]
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
    [Header("����")]
    public static Vector3 respawnPoint;



    [Header("DrinkDrug")]


    public static int DrinkCount=3;
    private bool isDrinking;
    [Header("Recovery")]
    public Text RecoveryCount;

    public GameObject[] stage;

    void Start()
    {
        
        // playerInventory.itemList[0] = default;
        MyInpulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isRun = true;
    }
    // Update is called once per frame
    private void Update()
    {
        FindBound();
        animatorMoveOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorMoveOverrideController;

        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadiu, Ground);
        //  isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadiu,Platform ) ;
        isTouchingFront = (Physics2D.OverlapCircle(frontCheck.position, checkRadiu, Wall|Ground));
        IsRun();
        WeaponSwitch();
        Jump();
        Roll();
        WallSlide();
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
    // private void Defense()
    // {
    //     if (Input.GetMouseButtonDown(1) && !isRoll && !isAttack && !isWallSliding && !isOnCorner)
    //     {
    //         isDefense = true;
    //         anim.SetBool("Defensing", true);
    //         rb.velocity = Vector2.zero;
    //     }
    //     if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDefensing"))
    //         anim.SetBool("isDefensed", true);
    //     if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDefensed"))
    //         anim.SetBool("isDefensed", false);
    //     if (Input.GetMouseButtonUp(1) && !isRoll && !isAttack && !isWallSliding && !isOnCorner)
    //     {
    //         isDefense = false;
    //         anim.SetBool("Defensing", false);
    //         anim.SetBool("isDefensed", false);
    //     }
    // }
    private void WallSlide()
    {
        if (isTouchingFront && !isGround && horizontalmove != 0 && !isClimbLadder)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("Sliding", false);
        }
        if (isWallSliding)
        {
            anim.SetBool("Sliding", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, 200));
        }

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
            SoundManager.instance.DownAttack();
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

            SoundManager.instance.Run();
        }
        else
        {
            SoundManager.instance.RunStop();
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && rollTimer > rollTime&&!isAttack&&!isOnCorner)
        {
            isRoll = true;
            rb.gravityScale = 0;
            if(!isWallSliding)
                targetPosition = new Vector3(-transform.localScale.x * rollDistance + transform.position.x,transform.position.y,transform.position.z);
            else
            { 
                isWallSliding = false;
                anim.SetBool("Sliding", false);
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y);
                targetPosition = new Vector3(transform.localScale.x * rollDistance + transform.position.x,transform.position.y,transform.position.z);
 
            }
            // rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            anim.SetTrigger("Rolling");
            SoundManager.instance.Roll();
            rollTimer = 0;
        }
        
        if (isRoll)
        {
            if(!isTouchingFront)
                transform.position = Vector3.MoveTowards(transform.position,targetPosition,rollSpeed*Time.deltaTime);
            ShadowPool.instance.GetFromPool();
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
            SoundManager.instance.Jump();
        }
        if (Input.GetButtonDown("Jump") && !isGround && JumpCount > 0 && !isAttack && !isWallSliding && !isOnCorner)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            anim.Play("PlayerJump", 0, 0f);
            JumpCount -= 1;
            SoundManager.instance.Jump();
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
            SoundManager.instance.Hurt();
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
            SoundManager.instance.Hurt();
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
        isDead = false;
        hp = maxHP;



        transform.position = respawnPoint;
        DrinkCount = 3;
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
        if (collision.CompareTag("Corner") && this.transform.localScale.x * collision.transform.localScale.x < 0 && !anim.GetBool("DownAttacking") && !isAttack)
        {

            rb.velocity = Vector2.zero;
            isOnCorner = true;
            this.transform.position = new Vector3(collision.transform.position.x - 2.3f, collision.transform.position.y + 19f, this.transform.position.z);
            anim.SetTrigger("OnCorner");
            rb.gravityScale = 0;
            //  Invoke("CornerMove", 0.45f);
        }

        //��ƽ̨
        if (collision.CompareTag("FragileFloor") && anim.GetBool("DownAttacking"))
        {
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("EnemyArrow"))
        {
            GetLightHit(collision.transform.parent.localScale.x, collision.GetComponentInParent<EnemyArrow>().attackPower);
        }
        //�жϹ������
        if (collision.CompareTag("EnemyTrigger"))
        {
            Debug.Log("123456");
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
                SoundManager.instance.Hurt();
            }
        }

    }
    

    public void AttackHorizentalMove(float attackMoveHorizentalSpeed)
    {
        rb.velocity = new Vector2(-transform.localScale.x * attackMoveHorizentalSpeed, rb.velocity.y);
    }//����ˮƽ�ƶ�
    public void AttackVerticalMove(float attackMoveVerticalSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, attackMoveVerticalSpeed);
    }//������ֱ�ƶ�
    private void WeaponSwitch()
    {
        anim.SetInteger("AttackID", id);
        switch (id)
        {
            case 1:
                Attack_001();
                break;
            case 2:
                Attack_002();
                break;
            case 3:
                Attack_003();
                break;
            case 4:
                Attack_004();//003=004
                break;
            case 5:
                Attack_005();
                break;
            case 6:
                Attack_006();
                break;
            case 7:
                Attack_007();//003=007
                break;
            case 8:
                Attack_008();
                break;
        }
    }
    private void Attack_001()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            if (comboStep > 3)
                comboStep = 1;

            if (comboStep != 3)
            {
                isHeavyAttack = false;
                if (comboStep == 1)
                {
                    AttackCalculation(1);
                }
                else
                {
                    AttackCalculation(2);
                }

                SoundManager.instance.Sword01();

            }
            else
            {
                isHeavyAttack = true;
                AttackCalculation(3);

                SoundManager.instance.Sword02();
            }



            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = interval;
            comboStep = 0;
        }

    }
    // public void InstantiateArrow(float addY)
    // {
    //     arrowPosition = new Vector3(transform.position.x - transform.localScale.x * 20f, transform.position.y + addY, transform.position.z);
    //     GameObject thisArrow = Instantiate(arrow, arrowPosition, transform.rotation);
    //     thisArrow.transform.localScale = transform.localScale;
    //     thisArrow.GetComponent<SpriteRenderer>().sprite = playerInventory.itemList[0].ArrowImage;
    //     thisArrow.GetComponent<Arrow>().attackPower = playerInventory.itemList[0].WeaponAttackPower[level - 1] * playerInventory.itemList[0].magnification[0];
    // }
    private void Attack_002()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {

            SoundManager.instance.Shoot();
            rb.velocity = Vector2.zero;
            isAttack = true;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Attacking");
        }
    }
    private void Attack_003()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            /*         if (comboStep != 2)
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed, rb.velocity.y);
                         SoundManager.instance.Sword01();

                     }
                     else
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed * 2f, rb.velocity.y);
                         SoundManager.instance.Sword02();
                     }*/


            if (comboStep > 2)
                comboStep = 1;
            if (comboStep == 1)
            {
                isHeavyAttack = false;
                AttackCalculation(1);
                SoundManager.instance.Axe01();
            }
            else
            {
                isHeavyAttack = true;
                AttackCalculation(2);
                SoundManager.instance.Axe01();
            }
            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        /*     timer -= Time.deltaTime;
             if (timer <= 0)
             {
                 timer = interval;
                 comboStep = 0;
             }*/
    }
    private void Attack_004()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            /*         if (comboStep != 2)
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed, rb.velocity.y);
                         SoundManager.instance.Sword01();

                     }
                     else
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed * 2f, rb.velocity.y);
                         SoundManager.instance.Sword02();
                     }*/


            if (comboStep > 2)
                comboStep = 1;
            if (comboStep == 1)
            {
                isHeavyAttack = false;
                AttackCalculation(1);
                SoundManager.instance.Lance01();
            }
            else
            {
                isHeavyAttack = true;
                AttackCalculation(2);
                SoundManager.instance.Lance01();
            }
            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        /*     timer -= Time.deltaTime;
             if (timer <= 0)
             {
                 timer = interval;
                 comboStep = 0;
             }*/
    }
    private void Attack_005()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isHeavyAttack = false;
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            if (comboStep > 3)
                comboStep = 1;

            if (comboStep != 3)
            {

                if (comboStep == 1)
                {
                    AttackCalculation(1);
                }
                else
                {
                    AttackCalculation(2);
                }
                rb.velocity = new Vector2(-transform.localScale.x * lightspeed, rb.velocity.y);
                SoundManager.instance.Sword01();

            }
            else
            {

                AttackCalculation(3);
                rb.velocity = new Vector2(-transform.localScale.x * lightspeed * 2f, rb.velocity.y);
                SoundManager.instance.Sword02();
            }



            timer = interval;
            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = interval;
            comboStep = 0;
        }
    }
    private void Attack_006()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            if (comboStep > 3)
                comboStep = 1;

            if (comboStep != 3)
            {
                isHeavyAttack = false;
                if (comboStep == 1)
                {
                    AttackCalculation(1);
                   
                }
                else
                {
                    AttackCalculation(2);
                 
                }

                SoundManager.instance.Shoot();

            }
            else
            {
                isHeavyAttack = true;
                AttackCalculation(3);
                Invoke("ConstantShoot",0);
                Invoke("ConstantShoot", 0.35f);
                Invoke("ConstantShoot", 0.7f);
            }



            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = interval;
            comboStep = 0;
        }

    }
    private void Attack_007()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            /*         if (comboStep != 2)
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed, rb.velocity.y);
                         SoundManager.instance.Sword01();

                     }
                     else
                     {
                         rb.velocity = new Vector2(-transform.localScale.x * lightspeed * 2f, rb.velocity.y);
                         SoundManager.instance.Sword02();
                     }*/


            if (comboStep > 2)
                comboStep = 1;
            if (comboStep == 1)
            {
                isHeavyAttack = false;
                AttackCalculation(1);
                SoundManager.instance.Axe01();
            }
            else
            {
                isHeavyAttack = true;
                AttackCalculation(2);
                SoundManager.instance.Axe02();
                Invoke("ConstantAxe", 0.5f);
            }
            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        /*     timer -= Time.deltaTime;
             if (timer <= 0)
             {
                 timer = interval;
                 comboStep = 0;
             }*/
    }
    private void Attack_008()
    {

        if (Input.GetMouseButtonDown(0) && !isRoll && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerLightHurt") && !isAttack && !isWallSliding && !isOnCorner)
        {
            isAttack = true;
            comboStep++;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            if (comboStep > 3)
                comboStep = 1;

            if (comboStep != 3)
            {
                isHeavyAttack = false;
                if (comboStep == 1)
                {
                    AttackCalculation(1);
                }
                else
                {
                    AttackCalculation(2);
                }

                SoundManager.instance.Lance01();

            }
            else
            {
                isHeavyAttack = true;
                AttackCalculation(3);

                SoundManager.instance.Lance02();
            }



            timer = interval;
            anim.SetTrigger("Attacking");
            anim.SetInteger("ComboStep", comboStep);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = interval;
            comboStep = 0;
        }

    }
    private void ConstantShoot()
    {
        SoundManager.instance.Shoot();
    }
    private void ConstantAxe()
    {
        SoundManager.instance.Axe01();
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
}