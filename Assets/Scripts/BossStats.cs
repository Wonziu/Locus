using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossStats : ScriptableObject   
{
    public string Name;
    public int StartHealthPoints;
    public Pickup Pickup;
    public float MovementSpeed;
    public float AttackRate;
    public float SpawningRocketsRate;
    public float SpawningEnemiesRate;
    public int RocketAmount;
}
