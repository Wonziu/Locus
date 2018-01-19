using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Boss
{
    private void Awake()
    {
        BossRigidbody2D = GetComponent<Rigidbody2D>();
        BossStateMachine = new StateMachine<Boss>(this);
    }

    protected override void Start()
    {
        base.Start();
        NextState = new ShootingState();
        BossStateMachine.ChangeState(new MovingState());     
    }

    private void Update()
    {
        BossStateMachine.UpdateState();
    }

    private void FixedUpdate()
    {
        BossStateMachine.FixedUpdateState();
    }  
}