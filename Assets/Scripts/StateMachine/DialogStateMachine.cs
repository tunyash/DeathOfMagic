using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStateMachine
{
    public static void Add(StateMachine stateMachine, RandomAccessMemory ram, 
        Animator animator, TextMesh dialogText, Mob mob, Rigidbody2D rigidbody2D, 
        SpeechDecisionMaker speechAnalyzer, float speed, IState inputState, IState outputState)
    {
        void AT(IState from, IState to, Func<bool> condition)
        {
            stateMachine.AddTransition(from, to, condition);
        }

        // dialog
        const float talkingDistance = 2f;
        var waveState = new WaveState(rigidbody2D, animator, ram).ToTimerState();
        var approachCompanionState = new MoveToTargetState(rigidbody2D, speed, () => ram.TalkingTarget?.Api?.Position, "talk");
        var talkState = new TalkState(animator, dialogText, speechAnalyzer, ram).ToTimerState();
        var listenState = new ListenState(ram);
        var endTalkingState = new EndDialogState(ram, animator);

        Func<bool> hasTalkingTarget = () => ram.TalkingTarget != null;
        Func<bool> talkingTargetIsInterested = () => ram.TalkingTarget?.Api.Talking.Target?.Equals(mob) ?? false;
        Func<bool> waveAnimationFinished = () => waveState.IsTimeout(2);
        Func<bool> gotCloseToTalkingTarget = () => (ram.TalkingTarget.Api.Position - rigidbody2D.position).sqrMagnitude <= talkingDistance;
        Func<bool> talkingTargetIsTalking = () => ram.TalkingTarget?.Api.Talking.CurrentPhrase != null;
        Func<bool> talkingAnimationFinished = () => talkState.IsTimeout(1.5f);
        Func<bool> saidBye = () => ram.CurrentPhrase?.IsGoodbye ?? false;
        Func<bool> heardBye = () => ram.LastHeardPhrase?.IsGoodbye ?? false;

        var exitTransition = Transitions.NotPredicate(talkingTargetIsInterested);
        AT(approachCompanionState, endTalkingState, exitTransition);
        AT(talkState, endTalkingState, Transitions.AndPredicate(talkingAnimationFinished, exitTransition));
        AT(listenState, endTalkingState, exitTransition);
        AT(waveState, endTalkingState, Transitions.AndPredicate(waveAnimationFinished, exitTransition));

        AT(waveState, approachCompanionState, Transitions.AndPredicate(waveAnimationFinished, talkingTargetIsInterested));
        AT(approachCompanionState, listenState, Transitions.AndPredicate(gotCloseToTalkingTarget, talkingTargetIsTalking));
        AT(approachCompanionState, talkState, Transitions.AndPredicate(gotCloseToTalkingTarget, Transitions.NotPredicate(talkingTargetIsTalking)));
        AT(talkState, listenState, Transitions.AndPredicate(talkingAnimationFinished, Transitions.NotPredicate(saidBye)));
        AT(talkState, endTalkingState, Transitions.AndPredicate(talkingAnimationFinished, saidBye));
        AT(listenState, talkState, Transitions.OrPredicate(Transitions.NotPredicate(talkingTargetIsTalking), heardBye));

        AT(inputState, waveState, hasTalkingTarget);
        AT(endTalkingState, outputState, Transitions.True);
    }

}
