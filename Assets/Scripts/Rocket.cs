using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MovingObject
{
    private Rigidbody2D myRigidbody2D;

    public float MovementSpeed;

	private void Awake()
	{
	    myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
    {
		CheckIfOutOfBorders();
	}

    public void SetNewRocket()
    {
        myRigidbody2D.velocity = new Vector2(0, -MovementSpeed);
    }
}
