using System;
using UnityEngine;

public class DayNight
{
    // Temporary Variable
    //public TextMeshProUGUI timerText;
    
    public event Action WhenDayBegins;
    public event Action WhenNightBegins;
    public bool IsDay { get; private set; } // 낮 = true, 밤 = false
    public int DayCount { get; private set; } // 날짜 수
    private HourMinute midNight0 = new HourMinute(0, 0); // 자정 0시
    private HourMinute midNight24 = new HourMinute(24, 0); // 자정 24시
    
    // 낮이 시작하는 시간
    private HourMinute timeWhenDayBegins;
    [Header("Day Begin")]
    [Range(0, 23)]
    public int hourWhenDayBegins = 8;
    [Range(0, 59)]
    public int minuteWhenDayBegins = 0;

    // 밤이 시작하는 시간
    private HourMinute timeWhenNightBegins;
    [Header("Night Begin")]
    [Range(0, 23)]
    public int hourWhenNightBegins = 20;
    [Range(0, 59)]
    public int minuteWhenNightBegins = 0;

    private int durationOfDay; // 낮이 지속되는 분 수
    private int durationOfNight; // 밤이 지속되는 분 수

    public float realSecondsForTimePass = 4f; // 시간이 흐를 때까지의 실제 초 수
    private float realSeconds; // 실제 초 수 저장
    private int minutesAfterDayNightChanged; // 낮밤이 바뀐 후의 분 수
    private readonly int TIME_PASS_MINUTES = 10; // 시간이 흐르는 분 간격
    public Clock gameClock;

    private readonly string dayText = " <sprite=1>";
    private readonly string nightText = " <sprite=3>";

    public DayNight()
    {
        timeWhenDayBegins = new HourMinute(hourWhenDayBegins, minuteWhenDayBegins);
        timeWhenNightBegins = new HourMinute(hourWhenNightBegins, minuteWhenNightBegins);

        if (timeWhenNightBegins >= timeWhenDayBegins)
        {
            durationOfNight = (midNight24 - timeWhenNightBegins) + (timeWhenDayBegins - midNight0);
            durationOfDay = timeWhenNightBegins - timeWhenDayBegins;
        }
        else
        {
            durationOfDay = (midNight24 - timeWhenDayBegins) + (timeWhenNightBegins - midNight0);
            durationOfNight = timeWhenDayBegins - timeWhenNightBegins;
        }

        gameClock = new Clock(hourWhenDayBegins, minuteWhenDayBegins);
        InitTime();
    }

    private void InitTime()
    {
        IsDay = true;
        DayCount = 1;
        realSeconds = 0;
    }

    public void FixedUpdate()
    {
        realSeconds += Time.deltaTime;
        if(realSeconds >= realSecondsForTimePass)
        {
            realSeconds -= realSecondsForTimePass;
            PassTime();
        }

        if (IsDay && minutesAfterDayNightChanged >= durationOfDay)
        {
            IsDay = false;
            minutesAfterDayNightChanged = 0;
            WhenNightBegins?.Invoke();
        }
        else if (!IsDay && minutesAfterDayNightChanged >= durationOfNight)
        {
            DayCount++;
            IsDay = true;
            minutesAfterDayNightChanged = 0;
            WhenDayBegins?.Invoke();
        }

        //timerText.text = GetTimerText();
    }

    public void PassTime()
    {
        gameClock.PassMinutes(TIME_PASS_MINUTES);
        minutesAfterDayNightChanged += TIME_PASS_MINUTES;
    }

    public string GetTimerText()
    {
        string text = "";
        text += "Day" + DayCount;
        if (IsDay)
        {
            text += dayText;
        }
        else
        {
            text += nightText;
        }

        text += '\n' + gameClock.GetClockText();
        return text;
    }
}