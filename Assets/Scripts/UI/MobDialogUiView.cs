using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDialogUiView : DialogUiViewBase
{
    [ SerializeField ] private TextMesh _mesh;

    private void Awake()
    {
        _mesh ??= GetComponent< TextMesh >();
        _mesh.gameObject.SetActive( false );
    }

    public override void StartDialog( IMob other )
    {
    }

    public override void Talk( Phrase phrase, IMob other )
    {
        _mesh.text = phrase?.Text;
        Debug.Log( $"({Name}) Talk {_mesh.text} to {(other as MonoBehaviour)?.name}");
        _mesh.gameObject.SetActive(true);
    }

    public override void Listen( Phrase phrase, IMob other )
    {
        Debug.Log($"({Name}) Listen {phrase.Text} from {(other as MonoBehaviour)?.name}");
        _mesh.gameObject.SetActive(false);
    }

    public override void Idle( IMob other )
    {
        Debug.Log($"({Name}) Idle");
        _mesh.gameObject.SetActive(false);
    }

    public override void EndDialog( IMob other )
    {
        _mesh.gameObject.SetActive(false);
    }

    private string Name => transform.parent.name;
}
