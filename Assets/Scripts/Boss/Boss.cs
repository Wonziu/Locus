using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

public class Boss : HostileCharacter
{
    [HideInInspector] public IState<Boss> NextState;
    [HideInInspector] public StateMachine<Boss> BossStateMachine;
    public WeaponStats BossWeaponStats;
    public BossStats MyBossStats;  
    public Weapon BossWeapon;
    public PoolManager BossPoolManager;
    public Vector3 EndPosition;
    public Transform PlayerTransform;
    public CooldownTimer ShootingCooldown;
    public CooldownTimer SpawningEnemiesCooldown;
    public CooldownTimer SpawningRocketsCooldown;
    public Spawner MySpawner;
    public int ShootsToChangeState;
    public int RocketSpawnsToChangeState;
    public int EnemiesSpawnsToChangeState;
    public float TimeBetweenStates;

    private new void Awake()
    {
        base.Awake();
        BossStateMachine = new StateMachine<Boss>(this);
    }

    private void Start()
    {
        SetValues();
        ShootingCooldown = new CooldownTimer(MyBossStats.AttackRate);
        SpawningRocketsCooldown = new CooldownTimer(MyBossStats.SpawningRocketsRate);
        SpawningEnemiesCooldown = new CooldownTimer(MyBossStats.SpawningEnemiesRate);
        NextState = new ShootingState();
        BossStateMachine.ChangeState(new MovingState());
    }

    private void SetValues()
    {
        maxHealthPoints = MyBossStats.StartHealthPoints;
        healthPoints = maxHealthPoints;
    }

    private void Update()
    {
        BossStateMachine.UpdateState();
    }

    private void FixedUpdate()
    {
        BossStateMachine.FixedUpdateState();
    }
}