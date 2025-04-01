using System;
using System.ComponentModel;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField]
    [ReadOnly(true)]
    int hp;
    [SerializeField]
    int maxHp;
    public delegate void OnAction();
    public delegate void OnHPChangeAction(int amountChanged);
    public event OnAction onDeath;
    public event OnAction onRevive;
    public event OnHPChangeAction onHPChange;

    private void Awake()
    {
        hp = maxHp;
    }
    public void Hurt(uint damage)
    {
        if (!IsDead())
        {
            hp -= Convert.ToInt32(damage);
            onHPChange?.Invoke(-Convert.ToInt32(damage));
            if (IsDead())
            {
                onDeath?.Invoke();
            }

        }
    }
    public void Heal(uint heal)
    {
        if (!IsDead())
        {
            int healAmount = Convert.ToInt32(heal);
            int estimatedHealResult = healAmount + hp;
            if (healAmount+hp > maxHp)
            {
                hp = maxHp;
                onHPChange?.Invoke(healAmount-(estimatedHealResult-maxHp));
            }
            else
            {
                hp = estimatedHealResult;
                onHPChange?.Invoke(healAmount);
            }
        }    
    }
    public void Revive()
    {
        
        hp = maxHp;
        onHPChange?.Invoke(maxHp);
        onRevive?.Invoke();
    }
    public bool IsDead() { return hp <= 0; }
    public int GetHp() { return hp; }
    public int GetMaxHp() { return maxHp; }
}
