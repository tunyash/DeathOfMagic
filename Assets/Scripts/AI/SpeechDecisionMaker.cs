using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase
{
    public bool IsHello { get; set; }
    public bool IsGoodbye { get; set; }
    public string Text { get; set; }
    public Animations Animation { get; set; }
}

public interface ISpeechDecisionMaker
{
    IEnumerator GeneratePhrase(IMob thisMob, IMob targetMob, DialogMemory memory );
}

// todo to singleton
public class SpeechDecisionMaker : ISpeechDecisionMaker
{
    public static SpeechDecisionMaker Instance { get; }

    static SpeechDecisionMaker()
    {
        Instance = new SpeechDecisionMaker(  );
    }

    private readonly float _byeChance;
    private readonly bool _shouldWave;

    private readonly string[] _phrases = new[]
    {
        "Корова",
        "Очереди",
        "Ебаный МФЦ",
        "Крутой пацан",
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

    private SpeechDecisionMaker(float byeChance = 0.1f, bool shouldWave = true)
    {
        _byeChance = byeChance;
        _shouldWave = shouldWave;
    }

    public IEnumerator GeneratePhrase(IMob thisMob, IMob targetMob, DialogMemory memory )//, Phrase phrase )
    {
        yield return new WaitForSeconds( 2 );

        var isHello = memory.LastSaidPhrase == null;

        var isGoodbye = !isHello && ( DomMath.IsChance(_byeChance) || (memory.LastHeardPhrase?.IsGoodbye ?? false) );

        var animation = ( ( isHello || isGoodbye ) && _shouldWave ) ? Animations.Wave : ( isHello || isGoodbye ? Animations.None : Animations.Talk );

        memory.QueuedPhrase = new Phrase { IsGoodbye = isGoodbye, Text = isGoodbye ? _byeString : _phrases[Random.Range(0, _phrases.Length)], IsHello = true, Animation = animation };
    }

}
