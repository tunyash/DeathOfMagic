using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingApi
{
    private readonly RandomAccessMemory _ram;

    public IMob Target => _ram.TalkingTarget;
    public Phrase CurrentPhrase => _ram.CurrentPhrase;

    public TalkingApi(RandomAccessMemory ram)
    {
        _ram = ram;
    }
}

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
        Talking = new TalkingApi(_ram);
    }

    public TalkingApi Talking { get; }

    public Vector2 Position => _rigidbody.position;
}
