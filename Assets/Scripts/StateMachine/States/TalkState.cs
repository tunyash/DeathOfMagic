using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : IState
{
    private readonly Animator _animator;
    private readonly RandomAccessMemory _ram;
    private readonly DialogUiViewBase _dialogUi;

    public TalkState(Animator animator, RandomAccessMemory ram, DialogUiViewBase dialogUi)
    {
        _animator = animator;
        _ram = ram;
        _dialogUi = dialogUi;
    }

    public void Tick()
    {
        MobExtensions.Listen(_ram);
    }

    public void OnEnter()
    {
        _ram.Dialog.CurrentSaidPhrase = _ram.Dialog.QueuedPhrase;
        _ram.Dialog.QueuedPhrase = null;
        _dialogUi.Talk(_ram.Dialog.CurrentSaidPhrase, _ram.TalkingTarget);
        _animator.SetBool(Constants.AnimationParams.Talk, true);
    }


    public void OnExit()
    {
        _dialogUi.Idle(_ram.TalkingTarget);
        _ram.Dialog.CurrentSaidPhrase = null;
        _animator?.SetBool(Constants.AnimationParams.Talk, false);
    }
}
