using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : IDisposable
{
    private TriggerHandler _handler;

    private HashSet<GameObject> _visible = new HashSet<GameObject>();

    public Vision(TriggerHandler handler)
    {
        _handler = handler;

        handler.EnterCollision += OnCollisionEnter;
        handler.ExitCollision += OnCollisionExit;
    }

    private void OnCollisionExit(GameObject obj)
    {
        _visible.Add(obj);
    }

    private void OnCollisionEnter(GameObject obj)
    {
        _visible.Remove(obj);
    }

    public void Dispose()
    {
        _handler.EnterCollision -= OnCollisionEnter;
        _handler.ExitCollision -= OnCollisionExit;
    }

    public IEnumerable<GameObject> Visible => _visible;
}
