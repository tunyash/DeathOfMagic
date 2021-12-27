using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSet
{
    private HashSet<string> _set = new HashSet<string>();

    public bool IsTrigger(string name)
    {
        return _set.Contains(name);
    }

    public void Invoke(string name)
    {
        _set.Add(name);
    }

    public void Refresh()
    {
        _set.Clear();
    }
}

