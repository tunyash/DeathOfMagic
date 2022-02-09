using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    private static GamePause _instance;

    private HashSet< Mob > _mobs;
    //private Dictionary<>
    
    private void Awake()
    {
        _instance = this;
        _mobs = GameObject.FindObjectsOfType<Mob>().ToHashSet();
        Debug.Assert(_mobs?.Count > 0);
    }

    public static void SetPaused(bool value, params IMob[] args)
    {
        _instance.SetPausedPrivate(args, value);
    }

    public static void Switch(params IMob[] args)
    {
        _instance.SetPausedPrivate(args, null);
    }

    private void SetPausedPrivate(IMob[] mobs, bool? value)
    {
        IEnumerable<Mob> mobsToCheck = _mobs;

        if( mobs?.Length > 0 )
        {
            // todo player as well
            mobsToCheck = mobs.Select( m => m as Mob ).Where( m => m != null );
        }
        foreach (var mob in mobsToCheck)
        {
            mob.enabled = !value ?? !mob.enabled;
        }
    }
}
