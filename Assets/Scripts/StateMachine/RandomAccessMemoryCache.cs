using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAccessMemoryCache
{
    private Dictionary<GameObject, Rigidbody2D> _rigidbodies = new Dictionary<GameObject, Rigidbody2D>();

    public RandomAccessMemoryCache(RandomAccessMemory memory)
    {
        memory.ObjectAdded += OnObjectAdded;
        memory.ObjectRemoved += OnObjectRemoved;
    }

    private void OnObjectRemoved(GameObject obj)
    {
        throw new NotImplementedException();
    }

    private void OnObjectAdded(GameObject obj)
    {
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            _rigidbodies.Add(obj, rb);
        }
    }

    public Rigidbody2D GetRigidbody(GameObject obj)
    {
        return _rigidbodies.ContainsKey(obj) ? _rigidbodies[obj] : null;
    }
}
