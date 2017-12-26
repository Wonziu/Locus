using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : ScriptableObject
{
    public float StartHealthPoints;
    public float BetterPickupChance;
    public Sprite EnemySprite;
    public int CoinValue;
    public string[] ItemNames;
}
