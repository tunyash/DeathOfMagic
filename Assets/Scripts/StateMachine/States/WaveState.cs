using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaveState : IState
{
    private readonly Rigidbody2D _rigidbody;
    private readonly Animator _animator;
    private readonly RandomAccessMemory _ram;

    public WaveState( Rigidbody2D rigidbody, Animator animator, RandomAccessMemory ram)
    {
        _rigidbody = rigidbody;
        _animator = animator;
        _ram = ram;
    }

    public void Tick()
    {
        if (_ram.TalkingTarget == null)
        {
            return;
        }
        Debug.DrawLine(_ram.TalkingTarget.Api.Position, _rigidbody.position, Color.blue, 0.2f);
    }

    public void OnEnter()
    {
        _animator.SetBool(Constants.AnimationParams.Wave, true );
    }

    public void OnExit()
    {
        _animator.SetBool(Constants.AnimationParams.Wave, false );
    }
}
