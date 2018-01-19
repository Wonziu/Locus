using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float healthPoints;
    private float maxHealthPoints;

    public WeaponStats BossWeaponStats;
    public BossStats MyBossStats;

    public Weapon BossWeapon;
    public PoolManager BossPoolManager;
    public IState<Boss> NextState;
    public Vector3 EndPosition;
    public Rigidbody2D BossRigidbody2D;
    public StateMachine<Boss> BossStateMachine;
    public Transform PlayerTransform;
    public CooldownTimer ShootingCooldown;

    protected virtual void Start()
    {
        SetValues();
        ShootingCooldown = new CooldownTimer(MyBossStats.AttackRate);
    }

    private void SetValues()
    {
        maxHealthPoints = MyBossStats.StartHealthPoints;
        healthPoints = maxHealthPoints;
    }
}