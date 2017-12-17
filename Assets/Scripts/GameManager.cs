using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float gameWidth;
    private float timeStamp;
    private bool isGameLost;
    private int difficultyLevel;
    private int spawnCount;

    public int Points;
    public int IncreasingLevelNumber;
    public ObjectPool MyObjectPool;
    public float SpawnCooldown;
    [Range(3.5f, 10)]
    public float EndTime;
    public bool IsEndSlowed;
    public EnemyStats[] EnemiesStats;
    public Text PointsText;

    void Start()
    {
        gameWidth = Camera.main.orthographicSize - 0.2f;
    }

    void Update()
    {
        if (timeStamp < Time.time)
        {
            if (spawnCount == IncreasingLevelNumber)
            {
                difficultyLevel++;
                spawnCount = 0;
                IncreasingLevelNumber += IncreasingLevelNumber / 2;
            }

            SpawnEnemies();
            timeStamp = Time.time + SpawnCooldown;
        }
    }

    private void SpawnEnemies()
    {
        if (!isGameLost)
            for (int i = 0; i < 5; i++)
            {
                MovingObject enemy = MyObjectPool.GetPooledObject("enemy");

                enemy.transform.position = new Vector2(-gameWidth / 2 + i * gameWidth / 4, 2);
                enemy.gameObject.SetActive(true);
                SetEnemiesValues(enemy);
            }
        spawnCount++;
    }

    private void SetEnemiesValues(MovingObject enemy)
    {
        int i = difficultyLevel + Random.Range(0, 2);

        enemy.GetComponent<Enemy>().SetNewEnemy(EnemiesStats[i]);
    }

    public void EndGame()
    {
        // TODO sprawić żeby po restarcie wszysto się zrestartowało a przeciwnicy znów się spawnowali

        isGameLost = true;

        if (IsEndSlowed)
            StartCoroutine(SlowTime(EndTime));
    }

    public void PickupCoin(int v)
    {
        Points += v;
        PointsText.text = "Points: " + Points;
    }

    public void SpawnItem(Enemy enemy)
    {
        // Losowe item TODO

        MovingObject coin = MyObjectPool.GetPooledObject("coin");
        coin.transform.position = enemy.transform.position;
        coin.GetComponent<Coin>().Value = enemy.CoinValue;

        coin.gameObject.SetActive(true);
    }

    private IEnumerator SlowTime(float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            var scale = Mathf.Lerp(1, 0, elapsedTime / time);
            elapsedTime += Time.fixedDeltaTime;

            Time.timeScale = scale;
            yield return new WaitForEndOfFrame();
        }
    }
}
