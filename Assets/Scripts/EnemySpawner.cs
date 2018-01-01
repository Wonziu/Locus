using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int difficultyLevel;
    private int spawnCount;
    private int increasingLevelNumber;
    private float timeStamp;
    private float gameWidth;

    public bool IsGameLost;
    public float StartCooldown;
    public float Offset;
    public float SpawnCooldown;
    public float SpecialEnemyChance;
    public int IncreasingLevelNumberBase;
    public PoolManager MyPoolManager;
    public EnemyStats[] EnemiesStats;
    public EnemyStats[] SpecialEnemiesStats;

    private void Start()
    {
        gameWidth = Camera.main.orthographicSize - Offset;
        timeStamp = Time.time + StartCooldown;
        increasingLevelNumber = IncreasingLevelNumberBase;
    }

    public void RestartValues()
    {
        timeStamp = Time.time + StartCooldown;
        difficultyLevel = 0;
        spawnCount = 0;
        increasingLevelNumber = IncreasingLevelNumberBase;
        IsGameLost = false;
    }

    private void Update()
    {
        if (IsGameLost) // or bossfight
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