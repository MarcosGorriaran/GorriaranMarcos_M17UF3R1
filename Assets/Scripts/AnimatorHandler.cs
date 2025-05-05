using UnityEngine;

public abstract class AnimatorHandler : MonoBehaviour
{
    [SerializeField]
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
