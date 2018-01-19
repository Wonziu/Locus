using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : IState<Boss>
{
    public void EnterState(Boss owner)
    {
        
    }

    public void UpdateState(Boss owner)
    {
        owner.BossWeapon.Aim(owner.PlayerTransform);

        if (!owner.ShootingCooldown.IsOnCooldown())
            owner.BossWeapon.Shoot(owner.BossWeaponStats);      
    }

    public void UpdateInFixedState(Boss owner)
    {
        
    }

    public void ExitState(Boss owner)
    {
       
    }
}
