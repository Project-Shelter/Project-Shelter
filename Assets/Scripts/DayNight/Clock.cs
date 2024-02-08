using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock
{
    public bool is12HourClock;
    public HourMinute Time { get; private set; }

    public int Hour
    {
        get
        {
            if (is12HourClock)
            {
                if (Time.hour % 12 == 0)
                {
                    return 12;
                }
                else
                {
                    return Time.hour % 12;
                }
            }
            else
            {
                return Time.hour;
            }
        }
    }
    public int Minute
    {
        get
        {
            return Time.minute;
        }
    }

    public bool IsAM { get { return Time.hour < 12; } }

    public Clock(int _hour = 0, int _minute = 0, bool _is12HourClock = true)
    {
        Time = new HourMinute(_hour, _minute);
        is12HourClock = _is12HourClock;
    }

    public void PassMinutes(int _minute)
    {
        if (_minute < 0) return;

        Time.minute += _minute;
        if (Time.minute >= 60)
        {
            Time.hour += Time.minute / 60;
            Time.minute %= 60;
        }
        if (Time.hour >= 24)
        {
            Time.hour %= 24;
        }
    }

    public string GetClockText()
    {
        string clockText = "";
        if (is12HourClock)
        {
            clockText += IsAM ? "AM" : "PM";
            clockText += " ";
        }

        string hourText;
        if(Hour < 10)
        {
            hourText = "0" + Hour.ToString();
        }
        else
        {
            hourText = Hour.ToString();
        }

        string minuteText;
        if (Minute < 10)
        {
            minuteText = "0" + Minute.ToString();
        }
        else
        {
            minuteText = Minute.ToString();
        }

        clockText += hourText + ":" + minuteText;
        return clockText;
    }
}
