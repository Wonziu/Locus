using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int difficultyLevel;
    private int spawnCount;
    private int increasingLevelNumber;
    private float timeStamp;
    private float gameWidth;

    public bool isGameLost;
    public float Offset;
    public float SpawnCooldown;
    public float SpecialEnemyChance;
    public int IncreasingLevelNumberBase;
    public ObjectPool MyObjectPool;
    public EnemyStats[] EnemiesStats;
    public EnemyStats[] SpecialEnemiesStats;

    private void Start()
    {
        gameWidth = Camera.main.orthographicSize - Offset;

        increasingLevelNumber = IncreasingLevelNumberBase;
    }

    private void Update()
    {
        if (isGameLost) // or bossfight
            return;

        if (timeStamp < Time.time)
        {
            if (spawnCount == increasingLevelNumber)
            {
                difficultyLevel++;
                spawnCount = 0;
                increasingLevelNumber += increasingLevelNumber / 2;
            }

            SpawnEnemies();
            timeStamp = Time.time + SpawnCooldown;
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            MovingObject enemy = MyObjectPool.GetPooledObject("enemy");            

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