using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : IState<Boss>
{
    private int bulletCount;

    public void EnterState(Boss owner)
    {

    }

    public void UpdateState(Boss owner)
    {
        if (!owner.ShootingCooldown.IsOnCooldown())
        {
            owner.BossWeapon.Aim(owner.PlayerTransform);
            owner.BossWeapon.Shoot(owner.BossWeaponStats);
            bulletCount++;

            if (bulletCount >= owner.MyBossStats.ShootsToChangeState)
            {
                owner.BossStateMachine.ChangeState(new SpawingState());
            }
        }
    }

    public void UpdateInFixedState(Boss owner)
    {

    }
}
