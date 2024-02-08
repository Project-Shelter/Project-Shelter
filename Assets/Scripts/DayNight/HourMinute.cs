using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourMinute
{
    public int hour;
    public int minute;

    public HourMinute(int hour, int minute)
    {
        this.hour = hour;
        this.minute = minute;
    }

    public static bool operator >=(HourMinute hm1, HourMinute hm2)
    {
        if(hm1.hour > hm2.hour)
        {
            return true;
        }
        else if(hm1.hour == hm2.hour && hm1.hour >= hm2.hour)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator <=(HourMinute hm1, HourMinute hm2)
    {
        if (hm1.hour < hm2.hour)
        {
            return true;
        }
        else if (hm1.hour == hm2.hour && hm1.hour <= hm2.hour)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int operator -(HourMinute hm1, HourMinute hm2)
    {
        int result = 0;
        result += (hm1.hour - hm2.hour) * 60;
        result += hm1.minute - hm2.minute;
       
        return result;
    }
}