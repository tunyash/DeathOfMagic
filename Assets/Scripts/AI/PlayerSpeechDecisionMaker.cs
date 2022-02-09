using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeechDecisionMaker : ISpeechDecisionMaker
{
    private Phrase _saidPhrase;

    public PlayerSpeechDecisionMaker( IPlayerDialogUiView view)
    {
        view.OptionSelected += ViewOnOptionSelected;
    }

    private void ViewOnOptionSelected( Phrase phrase )
    {
        _saidPhrase = phrase;
    }

    public void StartDialog( IMob target )
    {
    }

    public void EndDialog()
    {
    }

    public Phrase GeneratePhrase( Phrase listenedPhrase )
    {
        var phraseTmp = _saidPhrase;
        _saidPhrase = null;
        return phraseTmp;
    }
}
