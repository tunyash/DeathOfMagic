using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase
{
    public bool IsGoodbye;
    public string Text;
}

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

    public Phrase GenerateSpeech()
    {
        var isGoodbye = DomMath.IsChance(_byeChance) || (_ram.LastHeardPhrase?.IsGoodbye ?? false);

        return new Phrase{IsGoodbye = isGoodbye, Text = isGoodbye ? _byeString : _phrases[Random.Range(0, _phrases.Length)] };
    }
}
