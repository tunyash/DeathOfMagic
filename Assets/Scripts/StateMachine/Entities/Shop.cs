using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float ApplePrice;

    [SerializeField] private GameObject[] _apples;

    public Stack<GameObject> Apples;

    private void Awake()
    {
        Apples = new Stack<GameObject>(_apples);
    }
}
