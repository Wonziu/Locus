using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossStats : ScriptableObject   
{
    public string Name;
    public int StartHealthPoints;
    public float MovementSpeed;
    public float AttackRate;
    public float SpawningRocketsRate;
    public float SpawningEnemiesRate;
    public int RocketAmount;
    public int ShootsToChangeState;
    public int RocketSpawnsToChangeState;
    public int EnemiesSpawnsToChangeState;
    public float TimeBetweenStates;
    public bool IsSpawningRockets;
    public bool IsSpawningEnemies;
}
