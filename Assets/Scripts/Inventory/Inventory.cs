using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory<TItem>
{
    public IEnumerable<TItem> Items { get; }
    public void Add(TItem item);
    public void Remove(TItem item);
    public event Action<TItem> ItemAdded;
    public event Action<TItem> ItemRemoved;
}

public class Inventory : IInventory<GameObject>
{
    private List<GameObject> _list = new List<GameObject>();

    public IEnumerable<GameObject> Items => _list;

    public event Action<GameObject> ItemAdded;
    public event Action<GameObject> ItemRemoved;

    public void Add(GameObject item)
    {
        _list.Add(item);
        ItemAdded?.Invoke(item);
    }

    public void Remove(GameObject item)
    {
        _list.Remove(item);
        ItemRemoved?.Invoke(item);
    }
}
