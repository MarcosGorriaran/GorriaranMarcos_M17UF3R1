using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ITargetable
{
    [SerializeField]
    HPManager _hitPointsManager;
    [SerializeField]
    Group _groupMember;
    [SerializeField]
    [InspectorName("Starting threat level")]
    float _threatLevel;
    [SerializeField]
    Weapon _charWeapon;
    public Group GroupMember { get => _groupMember; set => _groupMember = value; }
    public float ThreatLevel { get => _threatLevel; set => _threatLevel = value; }
    public HPManager HitPointsManager
    {
        get
        {
            return _hitPointsManager;
        }
        private set
        {
            _hitPointsManager = value;
        }
    }
    public Weapon Weapon
    {
        get
        {
            return _charWeapon;
        }
        private set
        {
            _charWeapon = value;
        }
    }
    protected virtual void Awake()
    {
        Weapon.Owner = this;
    }
    protected virtual void OnEnable()
    {
        HitPointsManager.onDeath += OnDeath;
        HitPointsManager.onRevive += OnRevive;
        HitPointsManager.onHPChange += OnHPChange;
    }
    protected virtual void OnDisable()
    {
        HitPointsManager.onHPChange -= OnHPChange;
        HitPointsManager.onRevive -= OnRevive;
        HitPointsManager.onHPChange -= OnHPChange;
    }
    protected abstract void OnDeath();
    protected abstract void OnRevive();
    protected abstract void OnHPChange(int hpChange);

}
