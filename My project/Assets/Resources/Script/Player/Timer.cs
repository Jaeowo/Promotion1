using JetBrains.Annotations;
using System;
using UnityEngine;

public abstract class Timer
{
    protected float initialTime;
    protected float Time { get; set; }

    public bool IsRunning { get; protected set; }

    public float Progress => Time / initialTime;

    public Action OnTimerStart = delegate { };
    public Action OnTimerStop = delegate { };

    protected Timer(float value)
    {
        initialTime = value;
        IsRunning = false;
    }

    public void Start()
    {
        Time = initialTime;
        if (!IsRunning)
        {
            IsRunning = true;
            OnTimerStart.Invoke();
        }
    }

    public void Stop()
    {
        if(IsRunning)
        {
            IsRunning = false;
            OnTimerStop.Invoke();
        }
    }

    public void Resume() => IsRunning = true;
    public void Pause() => IsRunning = false;

    public abstract void Tick(float deltaTime);

}


// (카운트다운/ 쿨다운) 쿨타임 타이머

public class CountdownTimer: Timer
{
    public CountdownTimer(float value) : base(value) { }


    public override void Tick(float deltaTime)
    {
        if(IsRunning && Time > 0)
        {
            Time -= deltaTime;
        }

        if (IsRunning && Time <= 0)
        {
            Stop();
        }
    }

    // 이거 람다가 아니라 Expression-bodied member
    // return Time <= 0; 의 getter함수랑 같다고 보면됨
    public bool IsFinished => Time <= 0;

    public void Reset() => Time = initialTime;

    public void Reset(float newTime)
    {
        initialTime = newTime;
        Reset();
    }
}

// Stopwatch Timer

public class StopwatchTimer : Timer
{
    // base(0)은 부모 생성자 호출하는 식
    // base (0) 이 Timer(0)과 같음
    public StopwatchTimer() : base(0) { }

    public override void Tick(float deltaTime)
    {
        if (IsRunning)
        {
            Time += deltaTime;
        }
    }

    public void Reset() => Time = 0;

    public float GetTime() => Time;
}