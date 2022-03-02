using System;
using UnityEngine;

public class MoveToTargetState : IState, INamedState
{
    private readonly Animator _animator;
    private readonly Rigidbody2D _rigidbody;
    private readonly float _speed;
    private readonly Func<Vector2?> _positionFunc;
    private readonly string _name;
    private readonly SpriteFlipper _flipper;
    private readonly RandomAccessMemory _ram;

    public MoveToTargetState(Animator animator, Rigidbody2D rigidbody2D, float speed, Func<Vector2?> positionFunc, string name = null, SpriteFlipper flipper = null, RandomAccessMemory ram = null)
    {
        _animator = animator;
        _rigidbody = rigidbody2D;
        _speed = speed;
        _positionFunc = positionFunc;
        _name = name;
        _flipper = flipper;
        _ram = ram;
    }

    public void OnEnter()
    {
        _animator.SetBool("walk", true);
    }

    public void OnExit()
    {
        _animator.SetBool("walk", false);
        if (_ram != null)
        {
            _ram.TargetPosition = null; // todo
        }
    }

    public void Tick()
    {
        var positionTo = _positionFunc();

        if (positionTo == null)
        {
            return;
        }

        var direction = positionTo.Value - _rigidbody.position;

        if (!Movements.MoveTo(_rigidbody, positionTo.Value, _speed))
        {
            _flipper?.Flip(direction.x < 0);
        }
    }

    public string Name => GetType().Name + (_name == null ? string.Empty : "_" + _name);
}
