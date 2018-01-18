using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : Boss
{
    private void Awake()
    {
        MyRigidbody2D = GetComponent<Rigidbody2D>();
        MyStateMachine = new StateMachine<Boss>(this);
    }

    private void Start()
    {
        NextState = new MovingState();
        MyStateMachine.ChangeState(new MovingState());     
    }

    private void Update()
    {
        MyStateMachine.UpdateState();
    }

    private void FixedUpdate()
    {
        MyStateMachine.FixedUpdateState();
    }
}