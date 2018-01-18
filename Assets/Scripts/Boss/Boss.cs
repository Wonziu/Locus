using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public IState<Boss> NextState;
    public Vector3 EndPosition;
    public float MovementSpeed;
    public Rigidbody2D MyRigidbody2D;
    public PlayerController MyPlayer;
    public StateMachine<Boss> MyStateMachine;
}