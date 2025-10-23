using UnityEngine;

namespace ShootemUp
{
    [CreateAssetMenu(fileName = "WeaponStrategy", menuName = "Scriptable Objects/WeaponStrategy")]
    public abstract class WeaponStrategy : ScriptableObject
    {
        [SerializeField] int _damage = 1;
        [SerializeField] float _fireRate = 0.01f;
        [SerializeField] protected float _projectileSpeed = 10f;
        [SerializeField] protected float _projectileLifetime = 4f;
        [SerializeField] protected GameObject _projectilePrefab;

        public int Damage { get { return _damage; } set { _damage = value; } }
        public float FireRate { get { return _fireRate; } set { _fireRate = value; } }

        public abstract void Initialize();

        public abstract void Fire(Transform firePoint, LayerMask layer, int damage);

        
    }
}
