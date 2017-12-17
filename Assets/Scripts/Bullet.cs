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

    public void SetBulletValues(BulletStats bs)
    {
        mySpriteRenderer.sprite = bs.BulletSprite;
        Damage = bs.BulletDamage;

        gameObject.SetActive(true);
        myRigidBody.velocity = new Vector2(0 , bs.BulletSpeed); 
    }

    private void Update()
    {
        CheckIfOutOfBorders();
    }
}