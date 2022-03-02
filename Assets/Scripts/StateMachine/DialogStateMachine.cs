using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogStateMachine
{
    public static void AddDialogs(this StateMachine machine, ISpeechDecisionMaker speech, DialogUiViewBase dialogUi, Rigidbody2D rigidbody, float speed, RandomAccessMemory ram, Animator animator, IState outputState, params IState[] inputStates)
    {
        Func<Vector2?> talkingTargetPositionFunc = () => ram.TalkingTarget?.Api.Position;

        var approachState = new MoveToTargetState(animator, rigidbody, speed, talkingTargetPositionFunc, "talk");
        var startState = new StartDialogState(ram, dialogUi, speech);
        var endState = new EndDialogState(ram, speech, dialogUi);
        var talkState = new TalkState(animator, ram, dialogUi);
        var listenState = new ListenState( ram, speech, dialogUi );

        Func<bool> hasOther = () => ram.TalkingTarget != null;
        Func<bool> otherIsMob = () => !ram.TalkingTarget.IsPlayer();
        Func<bool> otherIsPlayer = Transitions.Not( otherIsMob );
        Func<bool> approached = Transitions.TargetReached(rigidbody, talkingTargetPositionFunc, 1);
        Func<bool> figuredPhrase = () => ram.Dialog.QueuedPhrase != null;
        Func<bool> otherIsInterested = () => ram.TalkingTarget?.Api.Talking.Target?.Equals(ram.ThisMob) ?? false;
        Func<bool> otherIsTalking = () => ram.TalkingTarget?.Api.Talking.CurrentPhrase != null;
        Func<bool> otherIsTalkingTrigger = () => ram.Dialog.IsListenTrigger();
        Func<bool> saidBye = () => ram.Dialog.CurrentSaidPhrase?.IsGoodbye ?? false;
        Func<bool> exitTransition = Transitions.Not(otherIsInterested);
        Func<bool> timeout = Transitions.StateTimeout(machine, 5);
        Func<bool> waitForTalk = Transitions.StateTimeout(machine, 2);

        foreach (var inputState in inputStates)
        {
            AT(machine, inputState, approachState, hasOther);
        }

        AT(machine, approachState, startState, approached);

        AT(machine, startState, listenState, otherIsTalking);
        AT(machine, startState, talkState, Transitions.And( figuredPhrase, Transitions.Not( otherIsTalking )));

        AT(machine, talkState, listenState, otherIsTalkingTrigger);
        AT(machine, talkState, endState, exitTransition);
        AT(machine, talkState, endState, saidBye);

        AT(machine, listenState, endState, exitTransition);

        AT(machine, endState, outputState, exitTransition);

        if( ram.ThisMob.IsPlayer() )
        {
            AT( machine, listenState, talkState, figuredPhrase );
        }
        else
        {
            AT( machine, startState, endState, timeout );
            AT( machine, approachState, endState, timeout );
            //AT( machine, listenState, talkState, Transitions.And( figuredPhrase, waitForTalk ));
            AT(machine, listenState, talkState, Transitions.And( figuredPhrase, otherIsPlayer ));
        }
    }

    private static void AT(StateMachine machine, IState from, IState to, Func<bool> condition)
    {
        machine.AddTransition(from, to, condition);
    }
}
