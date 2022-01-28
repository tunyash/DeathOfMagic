using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : IState
{
    private readonly Animator _animator;
    private readonly ISpeechDecisionMaker _speech;
    private readonly RandomAccessMemory _ram;

    public TalkState(Animator animator, ISpeechDecisionMaker speech, RandomAccessMemory ram)
    {
        _animator = animator;
        _speech = speech;
        _ram = ram;
    }

    public void Tick()
    {
        MobExtensions.Listen(_ram);
    }

    public void OnEnter()
    {
        _ram.Dialog.CurrentSaidPhrase = _ram.Dialog.QueuedPhrase;
        _ram.Dialog.QueuedPhrase = null;

        if (_ram.Dialog.CurrentSaidPhrase != null)
        {
            _animator.SetBool(Constants.AnimationParams.Talk, true);
        }
    }


    public void OnExit()
    {
        _ram.Dialog.CurrentSaidPhrase = null;
        _animator?.SetBool(Constants.AnimationParams.Talk, false);
    }
}
