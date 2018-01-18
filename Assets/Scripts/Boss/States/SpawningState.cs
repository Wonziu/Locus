using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningState : IState<Boss>
{
    public void EnterState(Boss owner)
    {
        Debug.Log("Wszedłem Spawn");
    }

    public void UpdateState(Boss owner)
    {
        Debug.Log("Update Spawn");
    }

    public void ExitState(Boss owner)
    {
        Debug.Log("Wyszedłem Spawn");
    }
}
