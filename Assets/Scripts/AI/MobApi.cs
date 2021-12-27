using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobApi
{
    private readonly RandomAccessMemory _ram;
    private readonly Animator _animator;
    private readonly Rigidbody2D _rigidbody;

    public MobApi(RandomAccessMemory ram, Animator animator, Rigidbody2D rigidbody)
    {
        _ram = ram;
        _animator = animator;
        _rigidbody = rigidbody;
    }

    public string AnimationState =>
        _animator.GetBool(Constants.AnimationParams.Wave)
            ? Constants.AnimationParams.Wave
            : (_animator.GetBool(Constants.AnimationParams.Talk) ? Constants.AnimationParams.Talk : Constants.AnimationParams.None);

    public IMob TalkingTarget => _ram.TalkingTarget;

    public Vector2 Position => _rigidbody.position;
}
