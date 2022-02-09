using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly DialogUiViewBase _dialogUi;
    private readonly ISpeechDecisionMaker _decisionMaker;

    private Coroutine _coroutine;

    public StartDialogState( RandomAccessMemory ram, DialogUiViewBase dialogUi, ISpeechDecisionMaker decisionMaker )
    {
        _ram = ram;
        _dialogUi = dialogUi;
        _decisionMaker = decisionMaker;
    }

    public void Tick()
    {
        MobExtensions.Listen( _ram );
        MobExtensions.GeneratePhrase(_decisionMaker, _ram);
    }

    public void OnEnter()
    {
        _ram.Dialog.Clear();

        if (_ram.TalkingTarget.Api.Talking.Target?.Api.Talking.Target == null)
        {
            _ram.TalkingTarget.Api.Talking.ForceTalk(_ram.ThisMob);
        }

        _decisionMaker.StartDialog(_ram.TalkingTarget);
        _dialogUi.StartDialog(_ram.TalkingTarget);
    }

    public void OnExit()
    {
    }
}
