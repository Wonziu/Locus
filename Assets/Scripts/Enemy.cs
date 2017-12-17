﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject
{
    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private float healthPoints;
    private float maxHealthPoints;

    public float BetterPickupChance;
    public int CoinValue;
    public float MovementSpeed = 1;
    public Image HealthBar;
    public GameManager MyGameManager;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        MyGameManager = FindObjectOfType<GameManager>();
    }

    private void GiveVelocity()
    {
        myRigidbody2D.velocity = new Vector2(0, -MovementSpeed);
    }

    private void ResetValues()
    {
        healthPoints = maxHealthPoints;
        UpdateHealthBar();
    }

    public void SetNewEnemy(EnemyStats so)
    {
        maxHealthPoints = so.StartHealthPoints;
        CoinValue = so.CoinValue;
        BetterPickupChance = so.BetterPickupChance; 
        mySpriteRenderer.sprite = so.EnemySprite;

        GiveVelocity();
        ResetValues();   
    }

    private void UpdateHealthBar()
    {
        HealthBar.fillAmount = healthPoints / maxHealthPoints;
    }

    public void TakeDamage(int d)
    {
        healthPoints -= d;
        UpdateHealthBar();

        if (healthPoints <= 0)
        {
            MyGameManager.SpawnItem(this);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckIfOutOfBorders();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Bullet")
        {
            TakeDamage(coll.GetComponent<Bullet>().Damage);            
            coll.gameObject.SetActive(false);
        }
    }
}
