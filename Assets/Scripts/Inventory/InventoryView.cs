using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{


    public void Add(GameObject itemIcon)
    {

    }

    public void Remove(GameObject itemIcon)
    {

    }

    public void Clear()
    {

    }

    public void Show()
    {

    }

    public void Hide()
    {

    }

    public string Caption { set; private get; }

    public string Description { set; private get; }

    public event Action<GameObject> ItemSelected;
}
