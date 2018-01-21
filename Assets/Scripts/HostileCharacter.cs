using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class HostileCharacter : MovingObject
{
    protected GameManager myGameManager;
    protected int healthPoints;
    protected int maxHealthPoints;

    public Action OnDeath;
    public string ParticleName;
    public Image HealthBar;
    [HideInInspector] public Rigidbody2D MyRigidbody2D;

    protected void Awake()
    {
        myGameManager = FindObjectOfType<GameManager>();
        MyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void TakeDamage(int d)
    {
        healthPoints -= d;
        UpdateHealthBar();

        if (healthPoints == 0)
        {
            KillEnemy();
            if (OnDeath != null)
                OnDeath.Invoke();
        }
    }

    protected void UpdateHealthBar()
    {
        HealthBar.fillAmount = (float)healthPoints / maxHealthPoints;
    }

    protected void KillEnemy()
    {
        myGameManager.SpawnParticle(transform.position, ParticleName);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "BulletPlayer")
        {
            TakeDamage(coll.GetComponent<Bullet>().Damage);
            coll.gameObject.SetActive(false);
        }
    }
}