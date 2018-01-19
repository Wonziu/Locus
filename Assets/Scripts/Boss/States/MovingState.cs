using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IState<Boss>
{
    private Vector3 startPos;
    private Vector3 destinationPoint;
  
    private Vector3 direction;
    private float percentBetweenwaypoints;
    private float distance;
    private float movementSpeed;

    public void EnterState(Boss owner)
    {
        startPos = owner.transform.position;
        destinationPoint = owner.EndPosition;
        movementSpeed = owner.MyBossStats.MovementSpeed;
        GetDirection();
    }

    public void UpdateState(Boss owner)
    {
       GetDirection();

        if (percentBetweenwaypoints >= 1)
            owner.BossStateMachine.ChangeState(owner.NextState);
    }

    private void GetDirection()
    {
        distance = Vector2.Distance(startPos, destinationPoint);
        percentBetweenwaypoints += Time.deltaTime * movementSpeed / distance;
        percentBetweenwaypoints = Mathf.Clamp01(percentBetweenwaypoints);
        direction = Vector2.Lerp(startPos, destinationPoint, percentBetweenwaypoints);
    }

    public void UpdateInFixedState(Boss owner)
    {
        owner.BossRigidbody2D.MovePosition(direction);
    }

    public void ExitState(Boss owner)
    {
        
    }
}