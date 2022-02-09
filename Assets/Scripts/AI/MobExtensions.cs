using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public static class MobExtensions
{
    public static void Listen( RandomAccessMemory ram )
    {
        ram.Dialog.CurrentHeardPhrase = ram.TalkingTarget?.Api.Talking?.CurrentPhrase;
    }

    public static bool IsListenTrigger(this DialogMemory dialog)
    {
        return dialog.CurrentHeardPhrase != null && !dialog.LastHeardPhrase.SafeEquals( dialog.CurrentHeardPhrase );
    }

    public static void GeneratePhrase(ISpeechDecisionMaker decisionMaker, RandomAccessMemory ram)
    {
        if( ram.Dialog.QueuedPhrase == null )
        {
            ram.Dialog.QueuedPhrase = decisionMaker.GeneratePhrase( ram.Dialog.CurrentHeardPhrase );
        }
    }

    public static bool IsPlayer( this IMob mob )
    {
        return ( mob as MonoBehaviour )?.gameObject.name == "Player";
    }
}
