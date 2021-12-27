using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Rect _rect;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Transform _transform;

    public static Room Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Rect Size => _rect;
}
