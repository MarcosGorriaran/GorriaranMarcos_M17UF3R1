using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Proyectile : MonoBehaviour
{
    Stack<Proyectile> _originPool;
    [SerializeField]
    uint _damage;
    Character _owner;
    [SerializeField]
    float _range;

    public float Range
    {
        get { return _range; }
        private set { _range = value; }
    }

    protected uint Damage
    {
        get { return _damage; }
        private set { _damage = value; }
    }
    public Stack<Proyectile> OriginPool
    {
        private get { return _originPool; }
        set { _originPool = value; }
    }
    public Character Owner
    {
        protected get { return _owner; }
        set { _owner = value; }
    }
    public virtual void AdjustRotation(Transform basedOn)
    {
        transform.position = basedOn.position;
        transform.LookAt(basedOn.position + basedOn.forward);
    }
    protected virtual void OnHit(Character hitedTarget)
    {
        hitedTarget.HitPointsManager.Hurt(Damage);
    }
    protected void Despawn()
    {
        OriginPool.Push(this);
        gameObject.SetActive(false);
    }
    protected bool IsTargetOwner(Character target)
    {
        return target.gameObject == Owner.gameObject;
    }
}
