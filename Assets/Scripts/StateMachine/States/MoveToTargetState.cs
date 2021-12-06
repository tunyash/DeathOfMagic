﻿using UnityEngine;

public class MoveToTargetState : IState
{
    private readonly Mob _mob;
    private readonly Rigidbody2D _rigidbody;
    private readonly float _speed;

    public MoveToTargetState(Mob mob, Rigidbody2D rigidbody2D, float speed)
    {
        _mob = mob;
        _rigidbody = rigidbody2D;
        _speed = speed;
    }

    public void OnEnter()
    {
        //throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        _mob.TargetPosition = null;
    }

    public void Tick()
    {
        var positionTo = _mob.TargetPosition.Value;

        var position = _rigidbody.position;
        var distance = (positionTo - position).magnitude;
        var direction = (positionTo - position).normalized;
        var speed = Mathf.Min(_speed * Time.deltaTime, distance);

        _rigidbody.position = position + speed * direction;
    }
}