using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DanceHandler : MonoBehaviour
{
    [SerializeField]
    AnimatorBooleanHandler _danceBooleanHandler;
    [SerializeField]
    float _duration;
    public UnityEvent startDancing;
    public UnityEvent endDancing;
    public event Action stopedDancing;
    public void StartDance()
    {
        _danceBooleanHandler.SwitchBool(true);
        StartCoroutine(Dancing());
        startDancing?.Invoke();
    }
    void StopDance()
    {
        _danceBooleanHandler.SwitchBool(false);
        endDancing?.Invoke();
        stopedDancing?.Invoke();
    }
    IEnumerator Dancing()
    {
        yield return new WaitForSeconds(_duration);
        StopDance();
    }
}
