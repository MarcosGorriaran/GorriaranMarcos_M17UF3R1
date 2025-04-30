using UnityEngine;

public class AnimatorBooleanHandler : AnimatorHandler
{
    [SerializeField]
    private string _boolParameterName;

    public bool GetBoolState()
    {
        return _animator.GetBool(_boolParameterName);
    }
    public void SwitchBool()
    {
        _animator.SetBool(_boolParameterName, !GetBoolState());
    }
    public void SwitchBool(bool value)
    {
        _animator.SetBool(_boolParameterName, value);
    }
}
