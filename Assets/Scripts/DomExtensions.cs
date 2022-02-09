using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DomExtensions
{
    public static bool SafeEquals( this Phrase phrase, Phrase other )
    {
        return phrase?.Equals( other ) ?? other == null;
    }
}
