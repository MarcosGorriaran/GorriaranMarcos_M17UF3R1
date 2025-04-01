using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CanISeeATarget", menuName = "ScriptableObjects/Conditions/Chase")]
public class ChaseCondition : ConditionSO
{
    protected override bool ProcessCheck(IACharacter ec)
    {
        return ec.TargetFinder.FoundTarget != null;
    }
}
