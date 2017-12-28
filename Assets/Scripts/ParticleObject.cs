using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleObject : MovingObject
{
    private ParticleSystem myParticleSystem;

    private void Awake()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!myParticleSystem.isEmitting)
            gameObject.SetActive(false);
    }
}
