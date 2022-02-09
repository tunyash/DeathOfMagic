using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coursor
{
    private readonly Collider2D _collider;
    private readonly Rigidbody2D _rigidbody;
    public IMob Selected { get; private set; }

    public bool Activated
    {
        set
        {
            _collider.enabled = value;
            if( !value )
            {
                Selected = null;
            }
            else
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if( Physics.Raycast( ray, out var hit ) )
                {
                    Selected = hit.transform.parent.GetComponent<IMob>();
                }
            }
        }
    }

    public Vector2 Position
    {
        set => _rigidbody.position = value;
    }

    public Coursor( TriggerHandler trigger )
    {
        _collider = trigger.GetComponent< Collider2D >();
        _rigidbody = trigger.GetComponent< Rigidbody2D >();
        trigger.EnterCollision += TriggerOnEnterCollision;
        trigger.ExitCollision += TriggerOnExitCollision;
        Activated = false;
    }

    private void TriggerOnExitCollision( GameObject obj )
    {
        Selected = null;
    }

    private void TriggerOnEnterCollision( GameObject obj )
    {
        Selected = obj.transform.parent.GetComponent< IMob >();
    }
}
