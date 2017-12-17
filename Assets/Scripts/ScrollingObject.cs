using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MovingObject
{
    public Vector2 Direction;

    private void Update()
    {
        CheckIfOutOfBorders();
        transform.Translate(Direction * Time.deltaTime);
    }
}
