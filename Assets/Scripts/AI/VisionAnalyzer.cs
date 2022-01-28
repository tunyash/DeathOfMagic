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
        _handler.ExitCollision += OnExitCollision;
    }

    private void OnExitCollision(GameObject obj)
    {
        if (obj.name.StartsWith("Mob"))
        {
            if (obj.name == "Mob bob")
            {
                _ram.DangerousTarget = null;
            }
            else if (obj.Equals((_ram.TalkingTarget as MonoBehaviour)?.gameObject))
            {
                _ram.TalkingTarget = null;
            }
        }
    }

    private void OnEnterCollision(GameObject obj)
    {
        if (obj.name.StartsWith("Mob"))
        {
            var mob = obj.GetComponent<IMob>();

            if (obj.name == "Mob bob")
            {
                _ram.DangerousTarget = mob;
                return;
            }

            if (_ram.TalkingTarget == null)
            {
                if (DomMath.IsChance(_chanceToTalk))
                {
                    if (mob.Api.Talking.Target?.Equals(_ram.ThisMob) ?? true)
                    {
                        _ram.TalkingTarget = mob;
                    }
                }
            }
        }
    }
}
