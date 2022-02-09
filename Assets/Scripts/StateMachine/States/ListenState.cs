using UnityEngine;

public sealed class ListenState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly ISpeechDecisionMaker _speechDecision;
    private readonly DialogUiViewBase _dialogUi;

    public ListenState(RandomAccessMemory ram, ISpeechDecisionMaker speechDecision, DialogUiViewBase dialogUi)
    {
        _ram = ram;
        _speechDecision = speechDecision;
        _dialogUi = dialogUi;
    }

    public void Tick()
    {
        MobExtensions.Listen( _ram );
        MobExtensions.GeneratePhrase( _speechDecision, _ram );

        if (_ram.Dialog.IsListenTrigger())
        {
            _dialogUi.Listen(_ram.Dialog.CurrentHeardPhrase, _ram.TalkingTarget);
        }
    }

    public void OnEnter()
    {
        _ram.Dialog.QueuedPhrase = null;

        if (_ram.Dialog.CurrentHeardPhrase != null)
        {
            _dialogUi.Listen(_ram.Dialog.CurrentHeardPhrase, _ram.TalkingTarget);
        }
    }

    public void OnExit()
    {
        _dialogUi.Idle(_ram.TalkingTarget);
    }
}
