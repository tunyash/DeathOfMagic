using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomAccessMemory
{
    public Vector2? TargetPosition;

    private HashSet<GameObject> _objects;

    public IEnumerable<GameObject> Objects => _objects;

    public void Add(GameObject gameObject)
    {
        _objects.Add(gameObject);
    }

    public void Remove(GameObject gameObject)
    {
        _objects.Remove(gameObject);
    }

    public event Action<GameObject> ObjectAdded;
    public event Action<GameObject> ObjectRemoved;
}
