using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movements
{
    public static bool MoveTo(Rigidbody2D rigidbody, Vector2? positionTo, float speed)
    {
        if (positionTo == null)
        {
            return false;
        }
        return MoveTo(rigidbody, positionTo.Value, speed);
    }

    public static bool MoveTo(Rigidbody2D rigidbody, Vector2 positionTo, float speed)
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

        rigidbody.position = ( position + speed * direction );
        return false;
    }

    public static void MoveTowards(Rigidbody2D rigidbody, Vector2 direction, float speed, bool isNormalized = true)
    {
        direction = isNormalized ? direction : direction.normalized;
        rigidbody.position = ( rigidbody.position + speed * direction );
    }

    public static void MoveTowards(Rigidbody2D rigidbody, Vector2? direction, float speed, bool isNormalized = true)
    {
        if (direction == null)
        {
            return;
        }

        MoveTowards(rigidbody, direction.Value, speed, isNormalized);
    }

    public static bool IsTargetReached(Rigidbody2D rigidbody, Vector2 position, float delta = 0.01f)
    {
        return (rigidbody.position - position).sqrMagnitude <= delta * delta;
    }
}
