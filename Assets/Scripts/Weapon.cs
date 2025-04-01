using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    const float RpmConversionRate = 60;
    [SerializeField]
    Transform _bulletSpawn;
    [SerializeField]
    WeaponSO _weaponInfo;
    Character _owner;
    Stack<Proyectile> _proyectilePool = new Stack<Proyectile>();
    Coroutine _weaponCooldown;


    public Character Owner
    {
        private get { return _owner; }
        set { _owner = value; }
    }
    public WeaponSO WeaponInfo {
        get 
        {
            return _weaponInfo;
        }
        private set
        {
            _weaponInfo = value;
        }
    }
    public void CreateBullet()
    {
        Proyectile newProyectile;
        if (_proyectilePool.Count > 0)
        {
            newProyectile = _proyectilePool.Pop();
            newProyectile.gameObject.SetActive(true);
        }
        else
        {
            newProyectile = Instantiate(WeaponInfo.BulletPrefab);
            newProyectile.OriginPool = _proyectilePool;
        }
        newProyectile.Owner = Owner;
        newProyectile.AdjustRotation(_bulletSpawn);
    }
    private IEnumerator WeaponCooldown()
    {
        yield return new WaitForSeconds(RpmToCooldownTime(_weaponInfo.RateOfFire));
        _weaponCooldown = null;
    }
    private float RpmToCooldownTime(float rpm)
    {
        return RpmConversionRate / rpm;
    }
    public void Fire()
    {
        if (_weaponCooldown == null)
        {
            _weaponCooldown = StartCoroutine(WeaponCooldown());
            CreateBullet();
        }
    }
}
