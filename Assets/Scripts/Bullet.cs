using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovingObject
{
    private SpriteRenderer mySpriteRenderer;
    private Rigidbody2D myRigidBody;

    public int Damage;
 
    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetBulletValues(WeaponStats ws)
    {
        mySpriteRenderer.sprite = ws.BulletSprite;
        Damage = ws.BulletDamage;

        gameObject.SetActive(true);
        myRigidBody.velocity = transform.up * ws.BulletSpeed; 
    }

    private void Update()
    {
        CheckIfOutOfBorders();
    }
}