using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : Proyectile
{
    private void OnEnable()
    {
        StartCoroutine(WiatForFrameBeforeProcess());
    }
    private Character LookForTarget()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray,out RaycastHit hit, Range) && hit.collider != null && hit.collider.gameObject.TryGetComponent(out Character hitChar) && !IsTargetOwner(hitChar))
        {
            return hitChar;
        }
        return null;
    }
    private IEnumerator WiatForFrameBeforeProcess()
    {
        yield return null;
        Character hitedTarget = LookForTarget();
        if (hitedTarget != null)
        {
            OnHit(hitedTarget);
        }
        Despawn();
    }
}
