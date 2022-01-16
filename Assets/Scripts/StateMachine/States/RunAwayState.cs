using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly Rigidbody2D _rigidbody2D;
    private readonly float _runSpeed;

    public RunAwayState(RandomAccessMemory ram, Rigidbody2D rigidbody2D, float runSpeed)
    {
        _ram = ram;
        _rigidbody2D = rigidbody2D;
        _runSpeed = runSpeed;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
        _ram.TargetPosition = null;
    }

    public void Tick()
    {
        var direction = _rigidbody2D.position - _ram.DangerousTarget?.Api.Position;
        Movements.MoveTowards(_rigidbody2D, direction, _runSpeed * Time.deltaTime, false);
    }

    private void DefineTargetPosition()
    {
    }
}
