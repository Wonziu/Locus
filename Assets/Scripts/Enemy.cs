using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : HostileCharacter
{ 
    private SpriteRenderer mySpriteRenderer;    
    private float movementSpeed = 1;

    [HideInInspector] public string[] ItemNames;
    [HideInInspector] public float BetterPickupChance;
    [HideInInspector] public int CoinValue;
    
    private new void Awake()
    {
        base.Awake();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        OnDeath = SpawnItem;
    }

    private void GiveVelocity()
    {
        MyRigidbody2D.velocity = new Vector2(0, -movementSpeed);
    }

    private void ResetValues()
    {
        healthPoints = maxHealthPoints;
        UpdateHealthBar();
    }

    public void SetNewEnemy(EnemyStats es)
    {
        maxHealthPoints = es.StartHealthPoints;
        CoinValue = es.CoinValue;
        BetterPickupChance = es.BetterPickupChance; 
        mySpriteRenderer.sprite = es.EnemySprite;
        ItemNames = es.ItemNames;

        GiveVelocity();
        ResetValues();   
    }

    public void SpawnItem()
    {
        float r = Random.Range(0, 1f);

        MovingObject item;

        if (BetterPickupChance > r)
        {
            string itemName = ItemNames[Random.Range(0, ItemNames.Length)];
            item = PoolManager.Instance.GetPooledObject(itemName);
        }
        else
        {
            item = PoolManager.Instance.GetPooledObject("coin");
            item.GetComponent<Coin>().Value = CoinValue;
        }

        item.transform.position = transform.position;
        item.gameObject.SetActive(true);
    }

    private void Update()
    {
        CheckIfOutOfBorders();
    }
}
