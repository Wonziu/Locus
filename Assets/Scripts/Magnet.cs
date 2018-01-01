using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{  
    private List<Pickup> pickUps;

    private void Awake()
    {
        pickUps = new List<Pickup>();
    }

    private void FixedUpdate()
    {
        foreach (var pickup in pickUps)
        {
            Vector3 forceDirection = transform.position - pickup.transform.position;            
            pickup.GetComponent<Rigidbody2D>().velocity = forceDirection.normalized * 4;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<Pickup>())
            pickUps.Add(coll.GetComponent<Pickup>());
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<Pickup>())
            pickUps.Remove(coll.GetComponent<Pickup>());
    }
}
