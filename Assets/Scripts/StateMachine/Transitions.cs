using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Transitions
{
    public static Func<bool> True = () => true;
    public static Func<bool> False = () => false;

    public static Func<bool> TargetDetected(RandomAccessMemory ram) => () => ram.TargetPosition.HasValue;

    public static Func<bool> TargetReached(Rigidbody2D rigidbody, Func<Vector2?> positionFunc, float delta = 0.01f) => () =>
    {
        var target = positionFunc();

        return target == null ? false : Movements.IsTargetReached(rigidbody, target.Value, delta);
    };

    public static Func<bool> TargetPositionReached(Rigidbody2D rigidbody, RandomAccessMemory ram) => TargetReached(rigidbody, () => ram.TargetPosition);
    
    public static Func<bool> CollidedObjectWithName(CollidedItems collided, string name) => () => collided.CollidedObjects.Any(o => o.name == name);

    public static Func<bool> AndPredicate(Func<bool> one, Func<bool> two) => () => one() && two();
    public static Func<bool> OrPredicate(Func<bool> one, Func<bool> two) => () => one() || two();
    public static Func<bool> NotPredicate(Func<bool> one) => () => !one();

    public static Func<bool> SeeDanger(RandomAccessMemory ram) => () => ram.DangerousTarget != null;

    /// <summary>
    /// Chance from 0 to 1
    /// </summary>
    /// <param name="value">From 0 to 1</param>
    /// <returns></returns>
    public static Func<bool> Chance(float value) => () => Random.Range(0f, 1f) <= value;


    public static Func<bool> TriggerToTransition(this TriggerSet triggers, string name) => () => triggers.IsTrigger(name);
    public static Func<bool> TriggerToTransition(this TriggerSet triggers, string name, float chance) => () => AndPredicate(TriggerToTransition(triggers, name), Chance(chance) )();
}

