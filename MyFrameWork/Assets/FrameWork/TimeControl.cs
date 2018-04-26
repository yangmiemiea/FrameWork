using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void CompleteEvent();

public delegate void UpdateEvent(float t);

public class TimeControl : BaseBehaviour
{
    bool isLog = true;

    UpdateEvent updateEvent;

    CompleteEvent onCompleted;

    float timeTarget;//计时时间

    float timeStart;//开始计时时间

    float timeNow;//现在时间

    float offestTime;//计时偏差

    float repateRate;

    bool isTimer;//是否开始计时

    bool isDestory = true;//计时结束后是否销毁

    bool isEnd;//计时是否结束

    bool isIgnoreTimeScale = true;//是否忽略时间速率

    bool isRepeate;                //是否重复

    float Time_
    {
        get { return isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time; }
    }
    float now;

    protected override void Update()
    {
        if (isTimer)
        {
            timeNow = Time_ - offestTime;//游戏运行时间 - 计时偏差
            now = timeNow - timeStart;// 已计时时间 = 当前时间 - 开始计时时间
            if (updateEvent != null)
                if (repateRate == 0)
                {
                    updateEvent(Mathf.Clamp01(now / timeTarget));
                }
                else 
                {
                    if (now % repateRate < 0.015f)
                    {
                        updateEvent(Mathf.Clamp01(now / timeTarget));
                    }
                }
            //如果当前已计时时间 大于 目标计时时间
            if (now > timeTarget)
            {
                if (onCompleted != null)
                    onCompleted();
                if (!isRepeate)
                    DestoryTimer();
                else
                    ReStartTimer();
            }
        }
    }

    public float GetLeftTime()
    {
        return Mathf.Clamp(timeTarget - now, 0, timeTarget);
    }

    private void OnApplicationPause(bool is_pause)
    {
        if (is_pause)
        {
            PauseTimer();
        }
        else
        {
            ContinueTimer();
        }
    }

    /// <summary>
    /// 继续计时
    /// </summary>
    private void ContinueTimer()
    {
        if (isEnd)
        {
            if (isLog) Debug.LogWarning("计时已经结束!请重新计时!");
        }
        else
        {
            if (!isTimer)
            {
                offestTime += (Time_ - _pauseTime);
                isTimer = true;
            }
        }
    }

    /// <summary>
    /// 计时结束
    /// </summary>
    public void DestoryTimer()
    {
        isTimer = false;
        isEnd = true;
        if (isDestory)
        {
            Destroy(gameObject);
        }
    }

    float _pauseTime;
    /// <summary>
    /// 暂停计时
    /// </summary>
    public void PauseTimer()
    {
        if (isEnd)
        {
            if (isLog) Debug.LogWarning("计时已经结束");
        }
        else
        {
            if (isTimer)
            {
                isTimer = false;
                _pauseTime = Time_;
            }
        }
    }

    /// <summary>
    /// 重新计时
    /// </summary>
    public void ReStartTimer()
    {
        timeStart = Time_;
        offestTime = 0;
    }

    /// <summary>
    /// 加（减）时
    /// </summary>
    /// <param name="time_"></param>
    public void ChangeTargetTime(float time_)
    {
        timeTarget += time_;
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    /// <param name="time_">总时间</param>
    /// <param name="onCompleted_">计时结束时回调</param>
    /// <param name="updateEvent_">每帧的回调</param>
    /// <param name="repateRate_">updateEvent_调用速率 默认每帧一次</param>
    /// <param name="isIgnoreTimeScale_">是否忽略时间缩放</param>
    /// <param name="isRepeate_">是否重复</param>
    /// <param name="isDestory_">执行完是否销毁</param>
    public void StartTiming(float time_, CompleteEvent onCompleted_, UpdateEvent updateEvent_ = null
        , float repateRate_ = 0, bool isIgnoreTimeScale_ = true, bool isRepeate_ = false, bool isDestory_ = true)
    {
        timeTarget = time_;
        if (onCompleted_ != null)
            onCompleted = onCompleted_;
        if (updateEvent_ != null)
            updateEvent = updateEvent_;
        repateRate = repateRate_;
        isDestory = isDestory_;
        isIgnoreTimeScale = isIgnoreTimeScale_;
        isRepeate = isRepeate_;

        timeStart = Time_;
        offestTime = 0;
        isEnd = false;
        isTimer = true;
    }

    /// <summary>
    /// 创建计时器
    /// </summary>
    /// <param name="gobjName"></param>
    /// <returns></returns>
    public static TimeControl CreaterTimer(string gobjName = "Timer")
    {
        GameObject g = new GameObject(gobjName);
        TimeControl timer = g.AddComponent<TimeControl>();
        return timer;
    }

}

