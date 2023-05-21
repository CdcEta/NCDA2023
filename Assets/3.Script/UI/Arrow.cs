using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{


    public float shootSpeed;
    private bool isShoot;
    public LayerMask Obstacle;
    private float rangeAngle;
    public Item bowItem;
    public float attackPower;

    public float exitTime=5f;
    SpriteRenderer spriteRenderer;
    float r, g, b, a;
    // Start is called before the first frame update
    void Start()
    {
        isShoot = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        r = spriteRenderer.color.r;
        g = spriteRenderer.color.g;
        b = spriteRenderer.color.b;
    }

    // Update is called once per frame
    void Update()
    {

        Shoot();
        Destroy(gameObject, exitTime);
    }

    private void Invisual()
    {
        a -= 20;
        spriteRenderer.color = new Color(r, g, b, a);
    }
    void Shoot()
    {

        Invoke("Invisual", 0.6f);
        Invoke("Invisual", 0.65f);
        Invoke("Invisual", 0.7f);
        Invoke("Invisual", 0.75f);
        Destroy(gameObject,0.8f);
        if (Physics2D.OverlapCircle(transform.position, 0.1f, Obstacle))
        {
            if (isShoot)
            {
                rangeAngle = Random.Range(-30, 30);
                transform.Rotate(0, 0, rangeAngle);
            }
            isShoot = false;
        }
        if (isShoot)
            transform.position = new Vector3(transform.position.x - transform.localScale.x * shootSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        else
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isShoot && collision.CompareTag("Enemy"))
        {
            rangeAngle = Random.Range(-30, 30);
            transform.Rotate(0, 0, rangeAngle);
            transform.SetParent(collision.transform);
            Destroy(gameObject,0.2f);
            isShoot = false;
        }
    }

}
