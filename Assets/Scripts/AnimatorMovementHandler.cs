using UnityEngine;


public class AnimatorMovementHandler : AnimatorHandler
{
    [SerializeField]
    private string _directionXParameterName;
    [SerializeField]
    private string _directionYParameterName;

    public void ChangeAnimationDir(Vector2 dir)
    {
        _animator.SetFloat(_directionXParameterName, dir.x);
        _animator.SetFloat(_directionYParameterName, dir.y);
    }
    public void ChangeAnimationDir(float x, float y)
    {
        ChangeAnimationDir(new Vector2(x,y));
    }
}
