using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogUiViewBase : MonoBehaviour
{
    public abstract void StartDialog( IMob other );

    public abstract void Talk( Phrase phrase, IMob other );

    public abstract void Listen( Phrase phrase, IMob other );

    public abstract void Idle(IMob other);

    public abstract void EndDialog( IMob other );
}
