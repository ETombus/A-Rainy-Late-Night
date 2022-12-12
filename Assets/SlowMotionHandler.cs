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

    void Start()
    {
        normalTimeScale = Time.timeScale;
        normalDeltaTime = Time.fixedDeltaTime;
    }

    public IEnumerator SlowTime(float slowDownSpeed)
    {
        Debug.Log("execute");
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
        currentSlowTime = 0;

        while (currentSlowTime < 1)
        {
            currentSlowTime += slowDownSpeed / 10;
            currentSlowTime = Mathf.Clamp01(currentSlowTime);
            currentTimeScaleMultiplier = slowDownCurve.Evaluate(currentSlowTime);

            Time.timeScale = normalTimeScale * currentTimeScaleMultiplier;
            Time.fixedDeltaTime = normalDeltaTime * currentTimeScaleMultiplier;

            Debug.Log(currentSlowTime);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        if (Time.timeScale != normalTimeScale)
        {
            NormalSpeed();
        }
    }

    public void NormalSpeed()
    {
        StopAllCoroutines();
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = normalDeltaTime;
    }
}
