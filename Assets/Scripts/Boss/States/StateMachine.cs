using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private IState<T> currentState;

    public T Owner;

    public StateMachine(T owner)
    {
        Owner = owner;
        currentState = null;
    }

    public void ChangeState(IState<T> newState)
    {
        if (currentState != null)
            currentState.ExitState(Owner);
        currentState = newState;
        currentState.EnterState(Owner);
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.UpdateState(Owner);
    }
}
