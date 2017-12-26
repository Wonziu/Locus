using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public bool GoingUp;

    protected void CheckIfOutOfBorders()
    {
        if (GoingUp)
        {
            if (transform.position.y > 1.5)
                gameObject.SetActive(false);
        }
        else if (transform.position.y < -2.5)
            gameObject.SetActive(false);
    }
}
