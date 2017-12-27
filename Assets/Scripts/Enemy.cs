using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject
{
    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private float healthPoints;
    private float maxHealthPoints;
    private float movementSpeed = 1;

    [HideInInspector]
    public string[] ItemNames;
    [HideInInspector]
    public float BetterPickupChance;
    [HideInInspector]
    public int CoinValue;
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
        myRigidbody2D.velocity = new Vector2(0, -movementSpeed);
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
