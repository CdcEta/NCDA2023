using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E002 : Enemy
{
    // Start is called before the first frame update
    [Header("Arrow")]
    public GameObject arrow;
    private Vector3 arrowPosition;
    protected override void Start()
    {
        base.Start();
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Patrol();
        IsFind();
        Attack();
        PlayerCloseSlow();
        Hurt();
        Patrol();
        Check();
    }

    protected void InstantiateItem(float addY)
    {

        arrowPosition = new Vector3(transform.position.x - transform.localScale.x * 20f, transform.position.y + addY, transform.position.z);
            GameObject thisArrow = Instantiate(arrow, arrowPosition, transform.rotation);
            thisArrow.transform.localScale = transform.localScale;
            thisArrow.GetComponent<EnemyArrow>().attackPower = attackPower[level - 1];
    }
  
}
