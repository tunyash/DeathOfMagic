using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerDialogUiView
{
    event Action<Phrase> OptionSelected;
}

public class PlayerDialogUiView : DialogUiViewBase, IPlayerDialogUiView
{
    [ SerializeField ] private DialogButtons _buttons;
    [ SerializeField ] private TextMeshProUGUI _text;

    private INode< Phrase >[ ] _options;

    private void Awake()
    {
        _text.text = null;
        _buttons.ButtonClicked += ButtonsOnButtonClicked;
    }

    private void ButtonsOnButtonClicked( int index )
    {
        OptionSelected?.Invoke(_options[index].Value);
    }

    public override void StartDialog( IMob other )
    {
        _text.text = null;
        _buttons.Clear();
        GamePause.SetPaused( true );
        GamePause.SetPaused(false, other);
        gameObject.SetActive( true );
    }

    public override void Talk( Phrase phrase, IMob other )
    {
        Debug.Log( $"Me talk '{phrase.Text}'" );
    }

    public override void Listen( Phrase phrase, IMob other )
    {
        Debug.Log($"Other talk '{phrase.Text}'");
        _text.text = phrase.Text;
        if ( phrase is INode< Phrase > node )
        {
            Options = node.Child;
        }
    }

    public override void Idle( IMob other )
    {
    }

    public override void EndDialog( IMob other )
    {
        GamePause.SetPaused(false);
        gameObject.SetActive(false);
    }

    public event Action< Phrase > OptionSelected;

    private INode< Phrase >[ ] Options
    {
        set
        {
            _options = value;
            _buttons.Options = value.Select( o => o.Value.Text );
        }
    }
}
