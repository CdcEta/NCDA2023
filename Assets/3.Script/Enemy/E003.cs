using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E003 : Enemy
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
        Patrol();
        IsFind();
        Attack_2();
        PlayerCloseSlow();
        Hurt();
        Patrol();
        Check();
        IsFar();
    }


}
