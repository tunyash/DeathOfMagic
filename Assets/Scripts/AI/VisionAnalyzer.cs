using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAnalyzer
{
    private readonly TriggerHandler _handler;
    private readonly RandomAccessMemory _ram;
    private readonly float _chanceToTalk;

    public VisionAnalyzer(TriggerHandler handler, RandomAccessMemory ram, float chanceToTalk)
    {
        _handler = handler;
        _ram = ram;
        _chanceToTalk = chanceToTalk;

        _handler.EnterCollision += OnEnterCollision;
    }

    private void OnEnterCollision(GameObject obj)
    {
        if (obj.name.StartsWith("Mob"))
        {
            if (_ram.TalkingTarget == null)
            {
                if (Random.Range(0f, 1f) <= _chanceToTalk)
                {
                    var mob = obj.GetComponent<IMob>();

                    if (mob.Api.AnimationState == Constants.AnimationParams.None)
                    {
                        _ram.TalkingTarget = mob;
                    }
                }
            }
        }
    }
}
