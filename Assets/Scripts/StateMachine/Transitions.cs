using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Transitions
{
    public static Func<bool> TargetDetected(RandomAccessMemory ram) => () => ram.TargetPosition.HasValue;

    public static Func<bool> TargetReached(Rigidbody2D rigidbody, RandomAccessMemory ram, float delta = 0.5f) => () => ram.TargetPosition.HasValue && (ram.TargetPosition.Value - rigidbody.position).sqrMagnitude <= delta * delta;
    
    public static Func<bool> CollidedObjectWithName(CollidedItems collided, string name) => () => collided.CollidedObjects.Any(o => o.name == name);

    public static Func<bool> AndPredicate(Func<bool> one, Func<bool> two) => () => one() && two();
    public static Func<bool> OrPredicate(Func<bool> one, Func<bool> two) => () => one() || two();
    public static Func<bool> NotPredicate(Func<bool> one) => () => !one();
}

