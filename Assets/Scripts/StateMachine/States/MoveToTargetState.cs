using System;
using UnityEngine;

public class MoveToTargetState : IState, INamedState
{
    private readonly Rigidbody2D _rigidbody;
    private readonly float _speed;
    private readonly Func<Vector2?> _positionFunc;
    private readonly string _name;

    public MoveToTargetState(Rigidbody2D rigidbody2D, float speed, Func<Vector2?> positionFunc, string name = null)
    {
        _rigidbody = rigidbody2D;
        _speed = speed;
        _positionFunc = positionFunc;
        _name = name;
    }

    public void OnEnter()
    {
        //throw new System.NotImplementedException();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        var positionTo = _positionFunc();

        if (positionTo == null)
        {
            return;
        }

        Movements.MoveTowards(_rigidbody, positionTo.Value, _speed);
    }

    public string Name => GetType().Name + (_name == null ? string.Empty : "_" + _name);
}
