using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : IState
{
    private readonly Animator _animator;
    private readonly Rigidbody2D _rigidbody;
    private GameObject _talkBox;

    public TalkState(Animator animator, Rigidbody2D rigidbody)
    {
        _animator = animator;
        _rigidbody = rigidbody;
        _talkBox = GameObject.Instantiate( Resources.Load("Prefabs/TalkBox") as GameObject );
        _talkBox.SetActive(false);
    }

    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        _talkBox.SetActive(true);
        _talkBox.transform.position = _rigidbody.position + new Vector2(0, 1);
        _animator.SetBool(Constants.AnimationParams.Talk, true);
    }

    public void OnExit()
    {
        _talkBox.SetActive(false);
        _animator.SetBool(Constants.AnimationParams.Talk, false);
    }
}
