using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Timer
{
    public float time;
    public Action callback;
    public bool laptopPause;

    public Timer(float time, Action callback, bool laptopPause)
    {
        this.time = time;
        this.callback = callback;
        this.laptopPause = laptopPause;
    }

    public void UpdateTimer()
    {
        if (laptopPause && StartWindows.instance.IsShowedUI())
        {
            if (time < 1)
                time = 1;
            return;
        }
        if(time <= 0)
        {
            callback?.Invoke();
            TimersManager.instance.timers.Remove(this);
            return;
        }
        time -= Time.deltaTime;
    }
}
public class TimersManager : MonoBehaviour
{
    public static TimersManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public List<Timer> timers;

    private void Update()
    {
        for (int i = 0; i < timers.Count; i++)
            timers[i].UpdateTimer();
    }

    public void AddTimer(float time, Action callback, bool laptopPause)
    {
        Timer timer = new Timer(time, callback, laptopPause);
        timers.Add(timer);
    }
}
