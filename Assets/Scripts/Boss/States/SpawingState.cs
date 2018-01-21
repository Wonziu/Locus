using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawingState : IState<Boss>
{
    private int spawnCount;
    private bool spawningRockets;
    private bool spawningEnemies;

    public void EnterState(Boss owner)
    {
        spawningRockets = owner.MyBossStats.IsSpawningRockets;
        spawningEnemies = owner.MyBossStats.IsSpawningEnemies;
    }

    public void UpdateState(Boss owner)
    {
        if (spawningRockets)
            if (spawnCount <= owner.MyBossStats.RocketSpawnsToChangeState - 1)
            {
                if (!owner.SpawningRocketsCooldown.IsOnCooldown())
                {
                    for (int i = 0; i < owner.MyBossStats.RocketAmount; i++)
                    {
                        float x = Random.Range(-0.85f, 0.85f);
                        owner.MySpawner.SpawnRocket(new Vector3(x, 2));
                    }
                    spawnCount++;
                }
            }
            else
            {
                spawnCount = 0;
                spawningRockets = false;
                if (spawningEnemies)
                    owner.SpawningEnemiesCooldown.AddCooldown(owner.MyBossStats.TimeBetweenStates);
            }
        else if (spawningEnemies)
            if (spawnCount <= owner.MyBossStats.EnemiesSpawnsToChangeState - 1)
            {
                if (!owner.SpawningEnemiesCooldown.IsOnCooldown())
                {
                    owner.MySpawner.SpawnEnemies();
                    spawnCount++;
                }
            }
            else
            {
                spawningEnemies = false;
        }
        else
        {
            owner.ShootingCooldown.AddCooldown(owner.MyBossStats.TimeBetweenStates);
            owner.BossStateMachine.ChangeState(new ShootingState());
        }

    }

    public void UpdateInFixedState(Boss owner)
    {

    }
}
