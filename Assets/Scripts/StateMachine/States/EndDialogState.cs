using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly ISpeechDecisionMaker _decisionMaker;
    private readonly DialogUiViewBase _dialogUi;

    public EndDialogState(RandomAccessMemory ram, ISpeechDecisionMaker decisionMaker, DialogUiViewBase dialogUi)
    {
        _ram = ram;
        _decisionMaker = decisionMaker;
        _dialogUi = dialogUi;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _dialogUi.EndDialog(_ram.TalkingTarget);
        _decisionMaker.EndDialog();
        _ram.TalkingTarget = null;
        _ram.Dialog.Clear();
    }

    public void OnExit()
    {
    }
}
