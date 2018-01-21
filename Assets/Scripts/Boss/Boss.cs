using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

public class Boss : HostileCharacter
{
    [HideInInspector]
    public IState<Boss> NextState;
    [HideInInspector]
    public StateMachine<Boss> BossStateMachine;
    public WeaponStats BossLoot;
    public WeaponStats BossWeaponStats;
    public BossStats MyBossStats;
    public Weapon BossWeapon;
    public Vector3 EndPosition;
    public Transform PlayerTransform;
    public CooldownTimer ShootingCooldown;
    public CooldownTimer SpawningEnemiesCooldown;
    public CooldownTimer SpawningRocketsCooldown;
    public Spawner MySpawner;
    
    private new void Awake()
    {
        base.Awake();
        BossStateMachine = new StateMachine<Boss>(this);

        OnDeath += KillBoss;
    }

    private void Start()
    {
        SetValues();
        ShootingCooldown = new CooldownTimer(MyBossStats.AttackRate);

        if (MyBossStats.IsSpawningRockets)
            SpawningRocketsCooldown = new CooldownTimer(MyBossStats.SpawningRocketsRate);
        if (MyBossStats.IsSpawningEnemies)
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

    private void KillBoss()
    {
        MySpawner.EndBossFight();
        SpawnUpgrade();
    }

    private void SpawnUpgrade()
    {
        MovingObject item = PoolManager.Instance.GetPooledObject("weaponUpgrade");
        item.GetComponent<WeaponUpgrade>().WeaponStat = BossLoot;
        item.transform.position = transform.position;
        item.gameObject.SetActive(true);
    }
}