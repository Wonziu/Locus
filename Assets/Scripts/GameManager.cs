using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private float gameWidth;
    private float timeStamp;
    private bool isGameLost;
    private int difficultyLevel;
    private int spawnCount;
    private int increasingLevelNumber;

    public float Offset;
    public int Points;
    public int IncreasingLevelNumberBase;
    public ObjectPool MyObjectPool;
    public float SpawnCooldown;
    [Range(3.5f, 10)]
    public float EndTime;
    public bool IsEndSlowed;
    public EnemyStats[] EnemiesStats;
    public Text PointsText;

    void Start()
    {
        gameWidth = Camera.main.orthographicSize - Offset;
        increasingLevelNumber = IncreasingLevelNumberBase;
    }

    void Update()
    {
        if (isGameLost)
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
        int i = difficultyLevel + Random.Range(0, 2);

        enemy.SetNewEnemy(EnemiesStats[i]);
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
        PointsText.text = string.Format("{0} {1}", "Points", Points);
    } 

    public void SpawnItem(Enemy enemy)
    {
        float r = Random.Range(0, 1f);

        MovingObject item;

        if (enemy.BetterPickupChance > r)
        {
            string itemName = enemy.ItemNames[Random.Range(0, enemy.ItemNames.Length - 1)];
            item = MyObjectPool.GetPooledObject(itemName);
        }
        else
        {
            item = MyObjectPool.GetPooledObject("coin");
            item.GetComponent<Coin>().Value = enemy.CoinValue;
        }    

        item.transform.position = enemy.transform.position;
        item.gameObject.SetActive(true);
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
