using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerHandler : AnimatorHandler
{
    [SerializeField]
    string _propertyName;
    public void InvokeTrigger()
    {
        _animator.SetTrigger(_propertyName);
    }
}
