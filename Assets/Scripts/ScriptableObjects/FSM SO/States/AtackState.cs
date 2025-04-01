using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/State/Attack")]
public class AtackState : StateSO
{
    public override void OnStateEnter(IACharacter ec)
    {
        ec.Agent.updateRotation = false;
    }
    public override void OnStateExit(IACharacter ec)
    {
        ec.Agent.updateRotation = true;
    }
    public override void OnStateUpdate(IACharacter ec)
    {
        if (ec.TargetFinder.FoundTarget != null)
        {
            ec.IALookAt(ec.TargetFinder.FoundTarget.transform.position);
            ec.Weapon.transform.LookAt(ec.TargetFinder.FoundTarget.transform);
        }
        ec.Weapon.Fire();
        ec.CheckEndingConditions();
    }
}
