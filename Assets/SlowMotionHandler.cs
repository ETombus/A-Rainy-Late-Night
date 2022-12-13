using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionHandler : MonoBehaviour
{
    [SerializeField] AnimationCurve slowDownCurve;

    float normalTimeScale;
    float normalDeltaTime;

    float currentSlowTime;
    float currentTimeScaleMultiplier;

    public bool timeSlowed;

    void Start()
    {
        normalTimeScale = Time.timeScale;
        normalDeltaTime = Time.fixedDeltaTime;
    }

    public IEnumerator SlowTime(float slowDownSpeed)
    {
        timeSlowed = true;
        currentSlowTime = 0;

        while (currentSlowTime <= slowDownCurve.length)
        {
            currentSlowTime += slowDownSpeed/10;
            currentSlowTime = Mathf.Clamp01(currentSlowTime);
            currentTimeScaleMultiplier = slowDownCurve.Evaluate(currentSlowTime);

            Time.timeScale = normalTimeScale * currentTimeScaleMultiplier;
            Time.fixedDeltaTime = normalDeltaTime * currentTimeScaleMultiplier;

            yield return null;
        }
    }

    public IEnumerator SlowTime(float slowDownSpeed, float duration)
    {
        StartCoroutine(SlowTime(slowDownSpeed));

        yield return new WaitForSeconds(duration);

        if (Time.timeScale != normalTimeScale)
        {
            NormalSpeed();
        }
    }

    public void NormalSpeed()
    {
        StopAllCoroutines();
        timeSlowed = false;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = normalDeltaTime;
    }
}
