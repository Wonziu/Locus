using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public EnemySpawner MyEnemySpawner;
    public int Points;
    public ObjectPool MyObjectPool;
    [Range(3.5f, 10)]
    public float EndTime;
    public bool IsEndSlowed;
    public Text PointsText; 

    public void EndGame()
    {
        // TODO sprawić żeby po restarcie wszysto się zrestartowało a przeciwnicy znów się spawnowali

        MyEnemySpawner.isGameLost = true;

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
            string itemName = enemy.ItemNames[Random.Range(0, enemy.ItemNames.Length)];
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
