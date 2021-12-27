using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movements
{
    public static bool MoveTowards(Rigidbody2D rigidbody, Vector2 positionTo, float speed)
    {
        var position = rigidbody.position;
        var distance = (positionTo - position).magnitude;
        speed *= Time.deltaTime;

        if (distance <= speed)
        {
            rigidbody.position = positionTo;
            return true;
        }

        var direction = (positionTo - position).normalized;

        rigidbody.position = position + speed * direction;
        return false;
    }
}
