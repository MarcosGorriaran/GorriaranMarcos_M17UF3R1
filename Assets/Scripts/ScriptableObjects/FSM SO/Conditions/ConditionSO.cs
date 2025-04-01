using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionSO : ScriptableObject
{
    public bool CheckCondition(IACharacter ec)
    {
        return ProcessCheck(ec) == answer;
    }
    protected abstract bool ProcessCheck(IACharacter ec);
    [SerializeField]
    private bool answer;

}
