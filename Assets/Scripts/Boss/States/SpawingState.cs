using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawingState : IState<Boss>
{
    private int spawnCount;
    private bool spawningRockets = true;

    public void EnterState(Boss owner)
    {

    }

    public void UpdateState(Boss owner)
    {
        if (spawningRockets)
        {
            if (!owner.SpawningRocketsCooldown.IsOnCooldown())
            {
                for (int i = 0; i < owner.MyBossStats.RocketAmount; i++)
                {
                    float x = Random.Range(-0.85f, 0.85f);
                    owner.MySpawner.SpawnRocket(new Vector3(x, 2));
                }

                spawnCount++;
                if (spawnCount >= owner.RocketSpawnsToChangeState)
                {
                    spawningRockets = false;
                    spawnCount = 0;
                    owner.SpawningEnemiesCooldown.AddCooldown(owner.TimeBetweenStates);
                }
            }
        }
        else if (!owner.SpawningEnemiesCooldown.IsOnCooldown())
        {
            owner.MySpawner.SpawnEnemies();
            spawnCount++;

            if (spawnCount >= owner.EnemiesSpawnsToChangeState)
            {
                owner.BossStateMachine.ChangeState(new ShootingState());
                owner.ShootingCooldown.AddCooldown(owner.TimeBetweenStates);
            }
        }

    }

    public void UpdateInFixedState(Boss owner)
    {

    }
}
