using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer
{
    private float cooldownTime;
    private float timeStamp;

    public CooldownTimer(float f)
    {
        cooldownTime = f;
        timeStamp = Time.time + f;
    }

    public CooldownTimer(float f, float startTimer)
    {
        cooldownTime = f;
        timeStamp = Time.time + startTimer;
    }

    public void SetNewCooldown(float f)
    {
        cooldownTime = f;
    }

    public bool IsOnCooldown()
    {
        if (Time.time > timeStamp)
        {
            timeStamp = Time.time + cooldownTime;
            return false;
        }
        return true;
    }
}
