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
    void StartDialog(IMob target);
    void EndDialog();
    Phrase GeneratePhrase( Phrase listenedPhrase );
}

// todo to singleton
public class SpeechDecisionMaker : ISpeechDecisionMaker
{
    private readonly float _byeChance;
    private readonly MonoBehaviour _coroutineInvoker;
    private bool _saidHi;
    private IMob _target;
    private string _byeString = "До свидания";
    private string _hiString = "Привет";

    private readonly string[] _phrases = new[]
    {
        "Корова",
        "Очереди",
        "Ебаный МФЦ",
        "Крутой пацан",
        "Идея на 100000 долларов!",
        "Ой иди нахуй",
        "Очевидно",
        "Поживем - увидим",
        "А ты как думаешь?",
        "Готово?",
        "Что за подстава?"
    };


    private Phrase _generatedPhrase;
    private Coroutine _coroutine;

    private NodePhrase _node;

    public SpeechDecisionMaker(MonoBehaviour coroutineInvoker, float byeChance = 0.3f )
    {
        _byeChance = byeChance;
        _coroutineInvoker = coroutineInvoker;

        _node = new NodePhrase( new NodePhrase( 
                                               new NodePhrase( new NodePhrase(  ){Text = "Not much.."}, new NodePhrase() { Text = "Good.." }) { Text = "Sup?"}
                                               ){ IsHello = true, Text = "Hello"}, 
                                new NodePhrase(  ){IsGoodbye = true, Text = "Bye" } ){IsHello = true, Text = "HI!"};
    }

    public void StartDialog( IMob target )
    {
        _target = target;
        _saidHi = false;
    }

    public void EndDialog()
    {
        _target = null;
        _generatedPhrase = null;
        if ( _coroutine != null )
        {
            _coroutineInvoker.StopCoroutine(_coroutine);
        }

        _coroutine = null;
    }

    public Phrase GeneratePhrase( Phrase listenedPhrase )
    {
        return _target.IsPlayer() ? GenerateForPlayer( listenedPhrase ) : GenerateForMob( listenedPhrase );
    }

    private Phrase GenerateForPlayer(Phrase listenedPhrase)
    {
        if (listenedPhrase == null || listenedPhrase.IsHello)
        {
            if (!_saidHi)
            {
                _saidHi = true;
                return _node;
            }

        }

        if ( listenedPhrase == null )
        {
            return null;
        }

        if (listenedPhrase is INode<Phrase> nodePhrase)
        {
            if (nodePhrase.Child.Length > 0)
            {
                return nodePhrase.Child[Random.Range(0, nodePhrase.Child.Length - 1)].Value;
            }

            return new Phrase { IsGoodbye = true, Text = _byeString };
        }

        return new Phrase { Text = _phrases[Random.Range(0, _phrases.Length)] };
    }

    private Phrase GenerateForMob(Phrase listenedPhrase)
    {
        if( listenedPhrase == null || listenedPhrase.IsHello)
        {
            if( !_saidHi )
            {
                _saidHi = true;
                return new Phrase { IsHello = true, Text = _hiString };
            }

        }

        if (listenedPhrase?.IsGoodbye ?? false || DomMath.IsChance(_byeChance))
        {
            return new Phrase { IsGoodbye = true, Text = _byeString };
        }

        return new Phrase { Text = _phrases[Random.Range(0, _phrases.Length)] };
    }
}
