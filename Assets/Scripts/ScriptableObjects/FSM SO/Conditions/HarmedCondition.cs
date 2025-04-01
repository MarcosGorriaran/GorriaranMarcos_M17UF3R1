using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmIHurt", menuName = "ScriptableObjects/Conditions/HarmedCondition")]
public class HarmedCondition : ConditionSO
{
    const float PercentageMax = 100;
    [SerializeField]
    [Range(0,100)]
    private float _healthPercentageThreshold;
    protected override bool ProcessCheck(IACharacter ec)
    {
        int actualHP = ec.HitPointsManager.GetHp();
        int maxHP = ec.HitPointsManager.GetMaxHp();
        float healthPercentage =  (actualHP*PercentageMax)/maxHP;
        return healthPercentage <= _healthPercentageThreshold;
    }
}
