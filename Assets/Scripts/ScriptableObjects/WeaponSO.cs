using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponSO : ScriptableObject
{
    [SerializeField]
    [InspectorName("RPM")]
    float _rateOfFire;
    [SerializeField]
    Proyectile _bulletPrefab;
    public float RateOfFire
    {
        get { return _rateOfFire; }
        private set { _rateOfFire = value; }
    }
    public Proyectile BulletPrefab
    {
        get { return _bulletPrefab; }
        private set { _bulletPrefab = value; }
    }

}
