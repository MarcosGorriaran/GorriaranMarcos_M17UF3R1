using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public ConditionSO StartCondition;
    public List<ConditionSO> EndConditions;
    public abstract void OnStateEnter(IACharacter ec);
    public abstract void OnStateUpdate(IACharacter ec);
    public abstract void OnStateExit(IACharacter ec);

}
