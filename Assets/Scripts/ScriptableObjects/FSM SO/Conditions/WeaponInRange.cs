using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmIInRange", menuName = "ScriptableObjects/Conditions/WeaponInRange")]
public class WeaponInRange : ConditionSO
{
    protected override bool ProcessCheck(IACharacter ec)
    {
        if(ec.TargetFinder.FoundTarget == null) return false;
        return Vector3.Distance(ec.TargetFinder.FoundTarget.transform.position,ec.transform.position)<=ec.Weapon.WeaponInfo.BulletPrefab.Range;
    }
}
