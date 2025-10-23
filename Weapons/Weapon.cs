using UnityEngine;

namespace ShootemUp
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] WeaponStrategy _weaponStrategy;
        [SerializeField] protected Transform _firePoint;

        public WeaponStrategy WeaponStrat { get { return _weaponStrategy; } }

        public void SetWeaponStrategy(WeaponStrategy strategy)
        {
            _weaponStrategy = strategy;
            _weaponStrategy.Initialize();
        }
    }
}
