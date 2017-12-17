using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MovingObject
{
    private Rigidbody2D myRigidBody;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        myRigidBody.AddForce(new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(1.2f, 1.6f)), ForceMode2D.Impulse);
    }

    private void Update()
    {
        CheckIfOutOfBorders();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Boundary")
        {
            Vector2 v = myRigidBody.velocity;
            v.x = 0;
            myRigidBody.velocity = v;            
        }
    }
}
