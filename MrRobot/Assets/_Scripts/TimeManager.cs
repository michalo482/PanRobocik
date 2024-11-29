using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [SerializeField] private float resumeRate = 3;
    [SerializeField] private float pauseRate = 7;

    private float targetScaleTime = 1f;
    private float timeAdjustRate;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Mathf.Abs(Time.timeScale - targetScaleTime) > 0.05f)
        {
            float adjustRate = timeAdjustRate * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetScaleTime, adjustRate);
        }
        else
        {
            Time.timeScale = targetScaleTime;
        }
    }

    public void PauseTime()
    {
        timeAdjustRate = pauseRate;
        targetScaleTime = 0;
    }

    public void ResumeTime()
    {
        timeAdjustRate = resumeRate;
        targetScaleTime = 1;
    }

    public void SlowMotionFor(float seconds)
    {
        StartCoroutine(SlowTimeCo(seconds));
    }

    private IEnumerator SlowTimeCo(float seconds)
    {
        targetScaleTime = 0.5f;
        Time.timeScale = targetScaleTime;
        yield return new WaitForSecondsRealtime(seconds);
        ResumeTime();
    }
}
