using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode< out T >
{
    INode<T>[] Child { get; }
    T Value { get; }
}

public sealed class NodePhrase : Phrase, INode<Phrase>
{
    public INode<Phrase>[] Child { get; }
    public Phrase Value => this;

    public NodePhrase(params NodePhrase[] children)
    {
        Child = children;
    }
}
