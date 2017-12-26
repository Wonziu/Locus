using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public PlayerController MyPlayer;
    private List<Pickup> pickUps;

    private void Awake()
    {
        pickUps = new List<Pickup>();
    }

    private void Update()
    {
        foreach (var pickup in pickUps)
        {
            Vector3 forceDirection = transform.position - pickup.transform.position;            
            pickup.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Coin")
        {
            pickUps.Add(coll.GetComponent<Pickup>());
            Debug.Log(pickUps);
        }
    }
}
