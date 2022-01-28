using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _runSpeed;
    private readonly float _safeDistance;
    private Vector2? _position;

    public RunAwayState(RandomAccessMemory ram, Rigidbody2D rigidbody2D, float runSpeed, float safeDistance = 5)
    {
        _ram = ram;
        _rigidbody2D = rigidbody2D;
        _runSpeed = runSpeed;
        _safeDistance = safeDistance;
    }

    public void OnEnter()
    {
        _ram.TargetPosition = DefineTargetPosition();
    }

    public void OnExit()
    {
        _ram.TargetPosition = null;
    }

    public void Tick()
    {
        //if (_position == null || Movements.IsTargetReached(_rigidbody2D, _position.Value))
        //{
        //}

        Movements.MoveTo(_rigidbody2D, _ram.TargetPosition, _runSpeed);
    }

    private Vector2? DefineTargetPosition()
    {
        if (_ram.DangerousTarget == null)
        {
            return null;
        }

        var direction = _rigidbody2D.position - _ram.DangerousTarget.Api.Position;
        return _rigidbody2D.position + direction.normalized * _safeDistance;
    }
}
