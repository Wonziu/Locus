using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private int difficultyLevel;
    private int spawnCount;
    private int increasingLevelNumber;
    private float gameWidth;
    private float rocketCooldown;
    private CooldownTimer spawningEnemyCooldown;
    private CooldownTimer spawningRocketCooldown;

    public bool IsGameLost;
    public float StartSpawnCooldown;
    public float Offset;
    public float SpawnCooldown;
    public float SpecialEnemyChance;
    [Range(10, 100)]
    public int BaseRocketTimer;
    public int IncreasingLevelNumberBase;
    public PoolManager MyPoolManager;
    public EnemyStats[] EnemiesStats;
    public EnemyStats[] SpecialEnemiesStats;
    public Transform PlayerTransform;

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
        if (IsGameLost) // or bossfight
            return;

        if (!spawningEnemyCooldown.IsOnCooldown())
        {
            if (spawnCount == increasingLevelNumber)
            {
                difficultyLevel++;
                spawnCount = 0;
                increasingLevelNumber += increasingLevelNumber + 2;
                UIManager.Instance.UpdateWaveCount(difficultyLevel + 1);
            }
            SpawnEnemies();
        }

        if (!spawningRocketCooldown.IsOnCooldown())
            SpawnRocket();
    }

    private void SpawnRocket()
    {
        MovingObject rocket = MyPoolManager.GetPooledObject("rocket");
        rocket.gameObject.SetActive(true);
        rocket.transform.position = new Vector2(PlayerTransform.position.x, 2);
        rocket.GetComponent<Rocket>().SetNewRocket();

        rocketCooldown = BaseRocketTimer - difficultyLevel / 3 - Random.Range(0, BaseRocketTimer / 2) ;
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            MovingObject enemy = MyPoolManager.GetPooledObject("enemy");            

            enemy.transform.position = new Vector2(-gameWidth / 2 + i * gameWidth / 4, 2);
            enemy.gameObject.SetActive(true);
            SetEnemiesValues(enemy.GetComponent<Enemy>());
        }
        spawnCount++;
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