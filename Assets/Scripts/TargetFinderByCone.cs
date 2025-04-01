using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinderByCone : TargetFinder
{
    [SerializeField]
    float _visionRange;
    [SerializeField]
    float _visionAngle;
    [SerializeField]
    float _maxRadius;
    Physics _physics;
    protected override Collider[] LookTargetMethod()
    {
        RaycastHit[] hitElements =  _physics.ConeCastAll(transform.position,_maxRadius,transform.forward,_visionRange,_visionAngle);
        List<Collider> colliders = new List<Collider>();
        foreach(RaycastHit hit in hitElements)
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
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * _visionRange));
    }
}
