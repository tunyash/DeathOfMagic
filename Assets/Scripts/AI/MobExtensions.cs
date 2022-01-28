using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public static class MobExtensions
{
    public static void Listen( RandomAccessMemory ram )
    {
        if( ram.TalkingTarget == null )
        {
            return;
        }

        ram.Dialog.CurrentHeardPhrase = ram.TalkingTarget.Api.Talking?.CurrentPhrase;
    }

    public static Coroutine StartGeneratingPhrase(ISpeechDecisionMaker decisionMaker, RandomAccessMemory ram)
    {
        return ram.CoroutineInvoker.StartCoroutine( decisionMaker.GeneratePhrase( ram.ThisMob, ram.TalkingTarget, ram.Dialog ) );
    }

    public static void StopGeneratingPhrase(RandomAccessMemory ram, Coroutine coroutine)
    {
        ram.CoroutineInvoker.StopCoroutine( coroutine );
    }
}
