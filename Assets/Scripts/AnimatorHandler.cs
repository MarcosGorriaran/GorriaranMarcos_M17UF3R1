using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AnimatorHandler : MonoBehaviour
{
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
