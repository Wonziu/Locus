using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private float speed;
    private float horizontal;
    private float fireRate;
    private int lives = 1;
    private bool isVulnerable = true;

    public bool CanShot;
    public GameManager MyGameManager;
    public Magnet MyMagnet;
    public Transform[] Muzzle;
    public PlayerStats MyPlayerStat;
    public PoolManager MyPoolManager;
    public ParticleSystem DeathParticle;
    public WeaponStats MyWeaponStat;
    public int MagnetTime;
    public int InvulnerabilityTime;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetPlayerStats();
    }

    private void SetPlayerStats()
    {
        speed = MyPlayerStat.PlayerSpeed;
        fireRate = MyPlayerStat.SpeedAttack;
    }

    private void Update()
    {
        GetPlayerInput();

        if (CanShot && Input.GetKey(KeyCode.X))
        {
            StartCoroutine(ShootingCooldown());
            Shoot();
        }
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
        switch (coll.tag)
        {
            case "Coin":
                MyGameManager.PickupCoin(coll.GetComponent<Coin>().Value);
                coll.gameObject.SetActive(false);
                break;

            case "Enemy":
                if (!isVulnerable)
                    return;

                coll.GetComponent<Enemy>().KillEnemy();
                lives--;                
                MyGameManager.UpdateLives(lives);

                if (lives <= 0)
                    KillPlayer();
                else 
                    StartCoroutine(BecomeInvulnerable());

                break;
       
            case "Upgrade":
                MyWeaponStat = coll.GetComponent<WeaponUpgrade>().WeaponStat;
                coll.gameObject.SetActive(false);
                break;

            case "Magnet":
                MyMagnet.gameObject.SetActive(true);
                StartCoroutine(ItemTimer(MagnetTime, MyMagnet.gameObject));
                coll.gameObject.SetActive(false);
                break;

            case "Life":
                coll.gameObject.SetActive(false);
                lives++;
                MyGameManager.UpdateLives(lives);
                break;
        }
    }

    private void KillPlayer()
    {
        MyGameManager.EndGame();
        MovingObject mo = MyPoolManager.GetPooledObject("playerParticle");
        mo.transform.position = transform.position;
        mo.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator ShootingCooldown()
    {
        CanShot = false;
        yield return new WaitForSeconds(fireRate);
        CanShot = true;
    }

    private IEnumerator ItemTimer(int time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }

    private IEnumerator BecomeInvulnerable()
    {
        SetVulnerableValues(false, 0.5f);

        float timer = InvulnerabilityTime;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime; 
            MyGameManager.UpdateTimer(timer);
            yield return new WaitForEndOfFrame();
        }

        SetVulnerableValues(true, 1);
    }

    private void SetVulnerableValues(bool b, float f)
    {
        isVulnerable = b;
        MyGameManager.TimerText.enabled = !b;
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, f); // increaces opacity
    }
}