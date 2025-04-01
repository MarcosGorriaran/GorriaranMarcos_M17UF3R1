using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]  
public class TravelBullet : Proyectile
{
    float _lifeTime;
    [SerializeField]
    float _speed;
    Rigidbody _bulletBody;
    Coroutine _bulletLifeCountdown;
    private void Awake()
    {
        _bulletBody = GetComponent<Rigidbody>();
    }
    private void OnDisable()
    {
        if(_bulletLifeCountdown != null)
        {
            StopCoroutine(_bulletLifeCountdown);
        }
        _bulletBody.velocity = Vector3.zero;
    }
    public override void AdjustRotation(Transform basedOn)
    {
        base.AdjustRotation(basedOn);
        _lifeTime = Range / _speed;
        _bulletBody.velocity = transform.forward * _speed;
        _bulletLifeCountdown = StartCoroutine(BulletCountdown(_lifeTime));
    }
    private IEnumerator BulletCountdown(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Despawn();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Character targetHit) && !IsTargetOwner(targetHit))
        {
            OnHit(targetHit);
            
        }
        Despawn();
    }
}
