using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStateMachine
{
    public static void Add(StateMachine stateMachine, RandomAccessMemory ram, 
        Animator animator, ISpeechDecisionMaker speechAnalyzer, float speed, out IState startState, out IState endState)
    {
        void AT(IState from, IState to, Func<bool> condition)
        {
            stateMachine.AddTransition(from, to, condition);
        }

        startState = new StartDialogState( speechAnalyzer, ram );
        endState = new EndDialogState(ram, animator);

        var talkState = new TalkState(animator, speechAnalyzer, ram).ToTimerState();
        var listenState = new ListenState(ram, speechAnalyzer);

        Func< bool > isTalking = () => ram.Dialog.CurrentSaidPhrase != null;
        Func<bool> talkingTargetIsInterested = () => ram.TalkingTarget?.Api.Talking.Target?.Equals(ram.ThisMob) ?? false;
        Func<bool> talkingTargetIsTalking = () => ram.TalkingTarget?.Api.Talking.CurrentPhrase != null;
        Func<bool> saidBye = () => ram.Dialog.CurrentSaidPhrase?.IsGoodbye ?? false;

        var exitTransition = Transitions.NotPredicate(talkingTargetIsInterested);

        AT(startState, listenState, talkingTargetIsTalking);
        AT(startState, talkState, Transitions.NotPredicate(talkingTargetIsTalking));
        AT(startState, endState, exitTransition);

        AT(talkState, listenState, Transitions.NotPredicate(isTalking));
        AT(talkState, endState, exitTransition );
        AT(talkState, endState, saidBye);

        AT(listenState, talkState, Transitions.NotPredicate(talkingTargetIsTalking));
        AT(listenState, endState, exitTransition );
    }

}
