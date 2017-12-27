using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private float speed;
    private float horizontal;
    private float fireRate;
    private bool hasShield;

    public bool CanShot;
    public GameManager MyGameManager;
    public Magnet MyMagnet;
    public Transform[] Muzzle;
    public PlayerStats MyPlayerStat;
    public ObjectPool MyObjectPool;
    public ParticleSystem DeathParticle;
    public WeaponStats MyWeaponStat;
    public int MagnetTime;

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

        var v3 = Input.mousePosition;
        v3.z = 10.0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MovingObject mo = MyObjectPool.GetPooledObject("coin");
            mo.transform.position = v3;
            mo.gameObject.SetActive(true);
        }
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
            MovingObject bullet = MyObjectPool.GetPooledObject("bullet");
            bullet.transform.position = Muzzle[i].position;
            bullet.GetComponent<Bullet>().SetBulletValues(MyWeaponStat);
        }
    }

    private IEnumerator ShootingCooldown()
    {
        CanShot = false;
        yield return new WaitForSeconds(fireRate);
        CanShot = true;
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
                if (hasShield)
                {
                    hasShield = false;
                    return;
                }

                MyGameManager.EndGame();
                Instantiate(DeathParticle, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        

            case "Upgrade":
                MyWeaponStat = coll.GetComponent<WeaponUpgrade>().WeaponStat;
                break;

            case "Magnet":
                MyMagnet.gameObject.SetActive(true);
                StartCoroutine(ItemTimer(MagnetTime, MyMagnet.gameObject));
                coll.gameObject.SetActive(false);
                break;

            case "Shield":
                hasShield = true;
                break;
        }
    }

    private IEnumerator ItemTimer(int time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }
}