using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinderByCapsule : TargetFinder
{
    [SerializeField]
    private float _detectionRadius;
    protected override Collider[] LookTargetMethod()
    {
        RaycastHit[] hitElements = Physics.SphereCastAll(transform.position,_detectionRadius,-transform.up);
        List<Collider> colliders = new List<Collider>();
        foreach (RaycastHit hit in hitElements)
        {
            if (!colliders.Contains(hit.collider))
            {
                colliders.Add(hit.collider);
            }

        }
        return colliders.ToArray();
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,_detectionRadius);
    }
}
