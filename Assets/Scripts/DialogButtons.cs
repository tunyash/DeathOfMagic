using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogButtons : MonoBehaviour
{
    [ SerializeField ] private Button _template;

    private List<Button> _buttons = new List<Button>(3);
    private List<TextMeshProUGUI> _texsts = new List<TextMeshProUGUI>(3);
    private int _count;

    public event Action< int > ButtonClicked;

    private void Awake()
    {
        for( int i = 0; i < _buttons.Capacity; i++ )
        {
            CreateButton();
        }
    }

    public void Clear()
    {
        foreach( var button in _buttons )
        {
            button.gameObject.SetActive( false );
        }
    }

    public IEnumerable< string > Options
    {
        set
        {
            Clear();
            for( int i = value.Count(); i > _buttons.Count; i-- )
            {
                CreateButton();
            }

            var index = 0;
            foreach( var option in value )
            {
                _texsts[ index ].text = option;
                _buttons[ index ].gameObject.SetActive( true );
                index++;
            }
        }
    }

    private void CreateButton()
    {
        var index = _buttons.Count;
        var cloneGameObject = GameObject.Instantiate(_template.gameObject);
        cloneGameObject.transform.SetParent( gameObject.transform );
        var cloneButton = cloneGameObject.GetComponent<Button>();
        var cloneText = cloneGameObject.GetComponentInChildren< TextMeshProUGUI >();
        _texsts.Add(cloneText);

        cloneButton.onClick.AddListener( () => OnClicked( index ) );
        cloneButton.gameObject.SetActive(false);
        _buttons.Add(cloneButton);
    }

    private void OnClicked( int index )
    {
        ButtonClicked?.Invoke( index );
    }
}
