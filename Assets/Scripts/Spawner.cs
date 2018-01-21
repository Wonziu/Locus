using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private bool bossFight;
    private int difficultyLevel;
    private int spawnCount;
    private int increasingLevelNumber;
    private float gameWidth;
    private float rocketCooldown;
    private CooldownTimer spawningEnemyCooldown;
    private CooldownTimer spawningRocketCooldown;
    private int bossCount;

    public bool IsGameLost;
    public float StartSpawnCooldown;
    public float Offset;
    public float SpawnCooldown;
    public float SpecialEnemyChance;
    [Range(10, 100)]
    public int BaseRocketTimer;
    public int IncreasingLevelNumberBase;
    public EnemyStats[] EnemiesStats;
    public EnemyStats[] SpecialEnemiesStats;
    public Transform PlayerTransform;
    public Vector3 BossPosition;
    public string[] BossNames;

    private void Start()
    {
        gameWidth = Camera.main.orthographicSize - Offset;
        increasingLevelNumber = IncreasingLevelNumberBase;

       SetCooldowns(); 
    }

    private void SetCooldowns()
    {
        rocketCooldown = BaseRocketTimer - difficultyLevel / 3 - Random.Range(0, BaseRocketTimer / 2);

        spawningEnemyCooldown = new CooldownTimer(SpawnCooldown, StartSpawnCooldown);
        spawningRocketCooldown = new CooldownTimer(rocketCooldown);
    }

    public void RestartValues()
    {        
        difficultyLevel = 0;
        spawnCount = 0;
        increasingLevelNumber = IncreasingLevelNumberBase;
        IsGameLost = false;
        SetCooldowns();
    }

    private void Update()
    {
        if (IsGameLost || bossFight) // or bossfight
            return;

        if (!spawningEnemyCooldown.IsOnCooldown()) 
        {
            SpawnEnemies();
            spawnCount++;

            if (spawnCount == increasingLevelNumber)
            {
                difficultyLevel++;
                spawnCount = 0;
                UIManager.Instance.UpdateWaveCount(difficultyLevel + 1);

                if ((difficultyLevel + 1) % 3 == 0)
                {
                    bossFight = true;
                    SpawnBoss();
                    bossCount++;
                }
            }
        }

        if (!spawningRocketCooldown.IsOnCooldown())
        {
            SpawnRocket(new Vector3(PlayerTransform.position.x, 2));
            rocketCooldown = BaseRocketTimer - difficultyLevel / 3 - Random.Range(0, BaseRocketTimer / 2);
        }
    }

    private void SpawnBoss()
    {
        MovingObject boss = PoolManager.Instance.GetPooledObject(BossNames[bossCount]);
        boss.transform.position = BossPosition;
        boss.GetComponent<Boss>().MySpawner = this;
        boss.GetComponent<Boss>().PlayerTransform = PlayerTransform;
        boss.gameObject.SetActive(true);
    }

    public void EndBossFight()
    {
        bossFight = false;
        spawningEnemyCooldown.AddCooldown(3);
    }

    public void SpawnRocket(Vector3 pos)
    {
        MovingObject rocket = PoolManager.Instance.GetPooledObject("rocket");
        rocket.transform.position = pos;
        rocket.gameObject.SetActive(true);
        rocket.GetComponent<Rocket>().SetNewRocket();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            MovingObject enemy = PoolManager.Instance.GetPooledObject("enemy");            

            enemy.transform.position = new Vector2(-gameWidth / 2 + i * gameWidth / 4, 2);
            enemy.gameObject.SetActive(true);
            SetEnemiesValues(enemy.GetComponent<Enemy>());
        }       
    }

    private void SetEnemiesValues(Enemy enemy)
    {
        float r = Random.Range(0, 1f);

        if (r < SpecialEnemyChance)
            enemy.SetNewEnemy(SpecialEnemiesStats[Random.Range(0, SpecialEnemiesStats.Length)]);
        else
        {
            int i = difficultyLevel + Random.Range(0, 2);

            enemy.SetNewEnemy(EnemiesStats[i]);
        }        
    }
}