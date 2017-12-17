using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private float speed;
    private float horizontal;
    private float moveVertical;
    private float fireRate;

    public bool CanShot;
    public GameManager MyGameManager;
    public Transform Muzzle;
    public PlayerStats MyPlayerStat;
    public ObjectPool MyObjectPool;
    public ParticleSystem DeathParticle;
    public BulletStats MyBulletStat;

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

        if (CanShot)
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
        MovingObject bullet = MyObjectPool.GetPooledObject("bullet");
        
        bullet.transform.position = Muzzle.position;
        bullet.GetComponent<Bullet>().SetBulletValues(MyBulletStat);
    }

    private IEnumerator ShootingCooldown()
    {
        CanShot = false;
        yield return new WaitForSeconds(fireRate);
        CanShot = true;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            MyGameManager.EndGame();
            Instantiate(DeathParticle, transform.position, Quaternion.identity);   
            gameObject.SetActive(false);
        }
        else if (coll.tag == "Coin")
        {
            MyGameManager.PickupCoin(coll.GetComponent<Coin>().Value);
            coll.gameObject.SetActive(false);
        }
    }
}