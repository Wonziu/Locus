using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Range(3.5f, 10)]
    public float EndTime;
    public bool IsEndSlowed;
    public int Points;
    public Spawner MySpawner;
    public PlayerController MyPlayer;
    
    public void PickupCoin(int v)
    {
        Points += v;
        UIManager.Instance.UpdatePoints(Points);
    }

    public void EndGame()
    {
        UIManager.Instance.EnableMenu(true);
        MySpawner.IsGameLost = true;

        if (IsEndSlowed)
            StartCoroutine(SlowTime(EndTime));
    }

    public void RestartGame()
    {
        Points = 0;
        MyPlayer.RestartValues();
        MySpawner.RestartValues();
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