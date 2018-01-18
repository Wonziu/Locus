﻿using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 startPos;
    private Rigidbody2D myRigidbody2D;
    private float speed;
    private float horizontal;
    private float magnetTime;
    private float fireRate;
    private bool isVulnerable = true;
    private CooldownTimer shootingCooldown;

    public GameManager MyGameManager;
    public Magnet MyMagnet;
    public Transform[] Muzzle;
    public PlayerStats MyPlayerStat;
    public PoolManager MyPoolManager;
    public WeaponStats MyWeaponStat;
    public int MagnetTime;
    public int Lives = 1;
    public float BonusfireRate;
    public float InvulnerabilityTime;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPos = transform.position;
        SetPlayerStats();
        StartCoroutine(BecomeInvulnerable());

        shootingCooldown = new CooldownTimer(fireRate);
    }

    private void SetPlayerStats()
    {
        speed = MyPlayerStat.PlayerSpeed;
        fireRate = MyPlayerStat.SpeedAttack;
    }

    private void Update()
    {
        GetPlayerInput();

        if (!shootingCooldown.IsOnCooldown() && isVulnerable)
            Shoot();
    }

    private void GetPlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal, 0);
        myRigidbody2D.velocity = movement * speed;
    }

    private void Shoot()
    {
        int count = Mathf.Clamp(MyWeaponStat.BulletAmount, 0, 3);

        for (int i = 0; i < count; i++)
        {
            MovingObject bullet = MyPoolManager.GetPooledObject("bullet");
            bullet.transform.position = Muzzle[i].position;
            bullet.GetComponent<Bullet>().SetBulletValues(MyWeaponStat);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isVulnerable)
            return;

        switch (coll.tag)
        {
            case "Coin":
                MyGameManager.PickupCoin(coll.GetComponent<Coin>().Value);
                coll.gameObject.SetActive(false);
                break;

            case "Enemy":       
                coll.GetComponent<Enemy>().KillEnemy();

                Lives--;
                UIManager.Instance.UpdateLives(Lives);

                if (Lives <= 0)
                    KillPlayer();
                else 
                    StartCoroutine(BecomeInvulnerable());

                CreateDeathParticle();
                break;

            case "Rocket":
                CreateDeathParticle();
                KillPlayer();
                break;
            
            case "Upgrade":                
                fireRate -= BonusfireRate;
                shootingCooldown.SetNewCooldown(fireRate);
                UIManager.Instance.UpdateBonusSpeedAttack(BonusfireRate);
                coll.gameObject.SetActive(false);
                break;

            case "Magnet":
                magnetTime = MagnetTime;

                if (!MyMagnet.gameObject.activeInHierarchy)
                {
                    MyMagnet.gameObject.SetActive(true);
                    StartCoroutine(MagnetTimer());
                }

                coll.gameObject.SetActive(false);
                break;

            case "Life":
                coll.gameObject.SetActive(false);
                Lives++;
                UIManager.Instance.UpdateLives(Lives);
                break;
        }
    }

    private void KillPlayer()
    {
        StopAllCoroutines();

        gameObject.SetActive(false);
        MyGameManager.EndGame();
    }

    private void CreateDeathParticle()
    {
        MovingObject mo = MyPoolManager.GetPooledObject("playerParticle");
        mo.transform.position = transform.position;
        mo.gameObject.SetActive(true);
    }
        
    private IEnumerator MagnetTimer()
    {
        while (magnetTime > 0)
        {
            magnetTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        MyMagnet.gameObject.SetActive(false);
    }

    private IEnumerator BecomeInvulnerable()
    {
        MyMagnet.gameObject.SetActive(false);
        SetVulnerableValues(false, 0.5f);

        float timer = InvulnerabilityTime;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime; 
            UIManager.Instance.UpdateTimer(timer);
            yield return new WaitForEndOfFrame();
        }

        SetVulnerableValues(true, 1);
    }

    private void SetVulnerableValues(bool b, float f)
    {
        isVulnerable = b;
        UIManager.Instance.TimerText.enabled = !b;
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, f); // increaces and decreases opacity
    }

    public void RestartValues()
    {
        transform.position = startPos;
        Lives = 1;
        gameObject.SetActive(true);
        StartCoroutine(BecomeInvulnerable());
    }
}