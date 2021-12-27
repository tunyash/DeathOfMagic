using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechDecisionMaker
{
    private readonly RandomAccessMemory _ram;
    private readonly float _byeChance;

    private readonly string[] _phrases = new[]
    {
        "Корова",
        "Очереди",
        "Ебаный МФЦ",
        "Фестивально!",
        "Крутой пацан",
        "Добавьте перца",
        "Посыпьте солью",
        "Идея на 100000 долларов!",
        "Ой иди нахуй",
        "Очевидно",
        "Поживем - увидим",
        "А ты как думаешь?",
        "Готово?",
        "Что за подстава?"
    };

    private string _byeString = "До свидания";

    public SpeechDecisionMaker(RandomAccessMemory ram, float byeChance = 0.1f)
    {
        _ram = ram;
        _byeChance = byeChance;
    }

    public string Analyze(out bool isGoodbye)
    {
        isGoodbye = DomMath.IsChance(_byeChance) || _ram.TalkingTarget.Api.TalkingTarget == null;

        if (isGoodbye)
        {
            return _byeString;
        }

        return _phrases[Random.Range(0, _phrases.Length)];
    }
}
