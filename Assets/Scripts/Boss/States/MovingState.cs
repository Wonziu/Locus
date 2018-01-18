using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IState<Boss>
{
    public void EnterState(Boss owner)
    {
        Debug.Log("Wszedłem ruszam");
    }

    public void UpdateState(Boss owner)
    {
        Debug.Log("Update ruszam");
    }

    public void ExitState(Boss owner)
    {
        Debug.Log("Wyszedłem ruszam");
    }
}
