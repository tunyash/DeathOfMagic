using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper
{
    //[SerializeField] private bool _defaultLeft;

    //private SpriteRenderer[] _sprites;
    private bool _value;
    private Transform _transform;

    public GameObject Parent 
    { 
        set
        {
            //_sprites = value.GetComponentsInChildren<SpriteRenderer>();
            _transform = value.transform;
        } 
    }


    public void Flip(bool isLeft)
    {
        if (isLeft == _value)
        {
            return;
        }

        _value = isLeft;

        //foreach (var sprite in _sprites)
        //{
        //    sprite.flipX = isLeft;
        //}

        var scale = _transform.localScale;
        scale.x = isLeft ? 1 : -1;

        _transform.localScale = scale;
    }
}
