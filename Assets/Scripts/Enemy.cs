using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject
{
    private GameManager myGameManager;
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
    public ParticleSystem DeathParticle;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myGameManager = FindObjectOfType<GameManager>();
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
            OnEnemyDeath();
    }

    private void OnEnemyDeath()
    {
        myGameManager.SpawnItem(this);
        myGameManager.SpawnEnemyParticle(this);
        gameObject.SetActive(false);
    }

    public void KillEnemy()
    {
        UpdateHealthBar();
        OnEnemyDeath();
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
