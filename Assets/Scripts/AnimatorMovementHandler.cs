using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorMovementHandler : MonoBehaviour
{
    [SerializeField]
    private string _directionXParameterName;
    [SerializeField]
    private string _directionYParameterName;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ChangeAnimationDir(Vector2 dir)
    {
        dir = dir;
        _animator.SetFloat(_directionXParameterName, dir.x);
        _animator.SetFloat(_directionYParameterName, dir.y);
    }
    public void ChangeAnimationDir(float x, float y)
    {
        ChangeAnimationDir(new Vector2(x,y));
    }
}
