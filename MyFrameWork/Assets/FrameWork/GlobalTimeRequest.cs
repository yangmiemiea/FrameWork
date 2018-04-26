using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalTimeRequest : BaseManager<GlobalTimeRequest> {

    protected override void Update()
    {
        //倒计时列表
        for (int i = 0; i < timerList.Count; i++)
        {
            if (timerList[i].shouldHappenTime < Time.realtimeSinceStartup)
            {
                timerList[i].action();
                if (!timerList[i].isRepeat)
                {
                    timerList.Remove(timerList[i]);
                }
                else
                {
                    timerList[i].shouldHappenTime = Time.realtimeSinceStartup + timerList[i].time;
                }
            }
        }
    }

    private static List<TimeEvent> timerList = new List<TimeEvent>();

    /// <summary>
    /// 倒计时
    /// </summary>
    /// <param name="time">时间</param>
    /// <param name="callBack">计时完成回调</param>
    /// <param name="isRepeat">是否重复</param>
    /// <returns></returns>
    public static TimeEvent AddDelayTime(float time, UnityAction callBack, bool isRepeat = false)
    {
        TimeEvent timer = new TimeEvent(Time.realtimeSinceStartup + time, time, callBack, isRepeat);
        timerList.Add(timer);
        return timer;
    }

    public static void CancleTime(TimeEvent timer)
    {
        if (timerList.Exists(a => a == timer))
        {
            timerList.Remove(timer);
        }
    }
}

public class TimeEvent
{
    public float shouldHappenTime;
    public float time;
    public UnityAction action;
    public bool isRepeat;

    public TimeEvent(float shouldHappenTime, float time, UnityAction action, bool isRepeat)
    {
        //执行时间
        this.shouldHappenTime = shouldHappenTime;
        //时间
        this.time = time;
        //回调
        this.action = action;
        //是否重复
        this.isRepeat = isRepeat;
    }
}
