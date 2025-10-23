using UnityEngine;

namespace ShootemUp
{
    [CreateAssetMenu(fileName = "SingleShot", menuName = "Scriptable Objects/SingleShot")]
    public class SingleShot : WeaponStrategy
    {
        public override void Initialize()
        {
            
        }
        public override void Fire(Transform firePoint, LayerMask layer, int damage)
        {
            var projectile = Instantiate(_projectilePrefab, firePoint.position,firePoint.rotation);
            projectile.transform.SetParent(firePoint);
            projectile.layer = layer;

            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.Speed = _projectileSpeed;
            projectileComponent.Damage = damage;

            Destroy(projectile, _projectileLifetime);
        }
    }
}
