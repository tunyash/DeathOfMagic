using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeData
{
    public DateTime RealTimeStart;
    public long PausedTotalSeconds;
}

public class TimeSettings
{
    public float GameMinuteRealSeconds = 1;
    public DateTime GameTimeStart = new DateTime(1, 1, 1, 1, 1, 1);
}

public class DomTime
{
    private static TimeSettings _timeSettings = new TimeSettings();

    public static DateTime GameTime => _timeSettings.GameTimeStart.AddSeconds(Time.time * (60 / _timeSettings.GameMinuteRealSeconds));
}
