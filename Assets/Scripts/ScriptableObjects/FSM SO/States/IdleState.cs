using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Idle", menuName = "ScriptableObjects/State/Idle")]
public class IdleState : StateSO
{
    public override void OnStateEnter(IACharacter ec)
    {
        ChangePatrolNode(ec);
    }
    public override void OnStateUpdate(IACharacter ec)
    {
        float dist = ec.Agent.remainingDistance;
        if (dist != Mathf.Infinity && ec.Agent.pathStatus == NavMeshPathStatus.PathComplete && ec.Agent.remainingDistance == 0)
        {
            ChangePatrolNode(ec);
        }
    }
    public override void OnStateExit(IACharacter ec)
    {
        ec.Agent.ResetPath();
    }
    private void ChangePatrolNode(IACharacter ec)
    {
        ec.Agent.SetDestination(NodeMapManager.Instance.GetRandomNode());
    }
}
