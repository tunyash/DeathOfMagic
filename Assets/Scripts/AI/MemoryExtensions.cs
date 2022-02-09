using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MemoryExtensions
{
    public static bool WasListenedPhraseChanged(this DialogMemory dialogMemory)
    {
        return !(dialogMemory.LastHeardPhrase.SafeEquals( dialogMemory.CurrentHeardPhrase ));
    }
    public static bool WasSaidPhraseChanged(this DialogMemory dialogMemory)
    {
        return !(dialogMemory.LastSaidPhrase.SafeEquals(dialogMemory.CurrentSaidPhrase));
    }
}
