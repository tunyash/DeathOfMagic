using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaveState : IState
{
    private readonly Mob _thisMob;
    private readonly Animator _animator;
    private readonly Vision _vision;
    private readonly RandomAccessMemory _ram;

    public WaveState( Mob thisMob, Animator animator, Vision vision, RandomAccessMemory ram)
    {
        _thisMob = thisMob;
        _animator = animator;
        _vision = vision;
        _ram = ram;
    }

    public void Tick()
    {
        Debug.DrawLine(_ram.TalkingTarget.Api.Position, _thisMob.Api.Position, Color.blue);
    }

    public void OnEnter()
    {
        _animator.SetBool(Constants.AnimationParams.Wave, true );
    }

    public void OnExit()
    {
        if (!(_ram.TalkingTarget?.Api?.TalkingTarget?.Equals(_thisMob) ?? false))
        {
            _ram.TalkingTarget = null;
        }

        _animator.SetBool(Constants.AnimationParams.Wave, false );
    }
}
