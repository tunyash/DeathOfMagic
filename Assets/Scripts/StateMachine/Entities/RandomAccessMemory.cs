using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogMemory
{
    private Phrase _currentSaid;
    private Phrase _currentHeard;

    public Phrase QueuedPhrase { get; set; }

    public Phrase LastSaidPhrase { get; private set; }
    public Phrase LastHeardPhrase { get; private set; }

    public Phrase CurrentSaidPhrase
    {
        get => _currentSaid;
        set
        {
            LastSaidPhrase = _currentSaid;
            _currentSaid = value;
        }
    }

    public Phrase CurrentHeardPhrase
    {
        get => _currentHeard;
        set 
        {
            LastHeardPhrase = _currentHeard;
            _currentHeard = value;
        }
    }

    public void Clear()
    {
        LastSaidPhrase = null;
        LastHeardPhrase = null;
        QueuedPhrase = null;
        CurrentSaidPhrase = null;
        CurrentHeardPhrase = null;
    }
}

public class RandomAccessMemory
{
    public RandomAccessMemory( IMob thisMob, MonoBehaviour coroutineInvoker)
    {
        ThisMob = thisMob;
        CoroutineInvoker = coroutineInvoker;
    }

    public IMob ThisMob { get; }

    public Vector2? TargetPosition { get; set; }

    public IMob TalkingTarget { get; set; }

    public IMob DangerousTarget { get; set; }

    public DialogMemory Dialog { get; set; } = new DialogMemory();

    public MonoBehaviour CoroutineInvoker { get; }
}
