using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Chase", menuName = "ScriptableObjects/State/Chase")]
public class ChaseState : StateSO
{
    public override void OnStateEnter(IACharacter ec)
    {
        
    }
    public override void OnStateUpdate(IACharacter ec)
    {
        if (ec.TargetFinder.FoundTarget != null)
        {
            ec.Agent.updateRotation = false;
            ec.IALookAt(ec.TargetFinder.FoundTarget.transform.position);
            ec.Agent.SetDestination(ec.TargetFinder.FoundTarget.transform.position);
            ec.CheckEndingConditions();
        }
    }
    public override void OnStateExit(IACharacter ec)
    {
        ec.Agent.updateRotation = true;
    }
}
