using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : IState
{
    private FSM manager;

    private Parameter parameter;

    private float timer;


    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter; 
    }
    public void OnEnter()
    {
        parameter.animator.Play("E003Idle");

    }
    public void OnUpdate() 
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
  
        if(parameter.target != null &&
            parameter.target.position.x >= parameter.chasePoints[0].position.x && 
            parameter.target.position.x <= parameter.chasePoints[1].position.x) 
        {
            manager.TransitonState(StateType.Chase);
        }
  
        if(timer >= parameter.idleTime)
        {
            manager.TransitonState(StateType.Patrol);
        }
    }
    public void OnExit() 
    {
        timer = 0;
    } 
}

public class PatrolState : IState
{
    private FSM manager;

    private Parameter parameter;

    private int patrolPosition;
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("E003Run");
    }
    public void OnUpdate() 
    {
        //达到巡逻点，旋转并返回
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);
        //缓慢移动到巡逻点
        manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);

        if (parameter.target != null &&
            parameter.target.position.x >= parameter.chasePoints[0].position.x &&
            parameter.target.position.x <= parameter.chasePoints[1].position.x)
        {
            manager.TransitonState(StateType.Chase);
        }

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) < .1f)
        {
            manager.TransitonState(StateType.Idle);
        }


    }
    public void OnExit()
    {
        patrolPosition++;
        if(patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0;
        }
    }
}


public class ChaseState : IState
{
    private FSM manager;

    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("E003Chase");
    }
    public void OnUpdate()
    {
        manager.FlipTo(parameter.target);
        if (parameter.target)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
            
        if(parameter.target == null||
            manager.transform.position.x < parameter.chasePoints[0].position.x||
            manager.transform.position.x > parameter.chasePoints[1].position.x) 
        {
            manager.TransitonState(StateType.Idle);
        }

        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            manager.TransitonState(StateType.Attack);
        }
    
    }
    public void OnExit() { }
}



public class AttackState : IState
{
    private FSM manager;

    private Parameter parameter;

    private AnimatorStateInfo info;
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("E003Attack");
    }
    public void OnUpdate() 
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
   
        if(info.normalizedTime>=.95f)
        {
            manager.TransitonState(StateType.Chase);
        }
    }
    public void OnExit() { }
}