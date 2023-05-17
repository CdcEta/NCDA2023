using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float shootSpeed;
    private bool isShoot;
    public LayerMask Obstacle;
    private float rangeAngle;
    public Item bowItem;
    public float attackPower;
    public float exitTime;
    // Start is called before the first frame update
    void Start()
    {

        isShoot = true;


    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Destroy(gameObject,exitTime);
    }

    void Shoot()
    {
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
        if (isShoot && collision.CompareTag("Player"))
        {
            transform.Rotate(0, 0, rangeAngle);
            transform.SetParent(collision.transform);
            Destroy(gameObject,0.2f);
           isShoot = false;
        }
    }

}
