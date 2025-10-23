using UnityEngine;

namespace ShootemUp
{
    [CreateAssetMenu(fileName = "MultiShot", menuName = "Scriptable Objects/MultiShot")]
    public class MultiShot : WeaponStrategy
    {
        [SerializeField] float _spreadAngle = 10f;
        [SerializeField] int _bullectCht = 1;

        public int BulletCht { get { return _bullectCht; } set {  _bullectCht = value; } }

        public override void Initialize()
        {

        }

        public override void Fire(Transform firePoint, LayerMask layer, int damage)
        {
            int projectileCount = BulletCht;
            float startAngle = -_spreadAngle * ( projectileCount - 1) / 2f;

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = startAngle + (_spreadAngle * i);
                Quaternion rotation = firePoint.rotation * Quaternion.Euler(0f, angle, 0f);

                var projectile =Instantiate(_projectilePrefab, firePoint.position, rotation);
                projectile.transform.SetParent(firePoint);
                projectile.layer = layer;

                var projecttileCoponent = projectile.GetComponent<Projectile>();
                projecttileCoponent.Speed = _projectileSpeed;
                projecttileCoponent.Damage = damage;

                Destroy(projectile, _projectileLifetime);
            }
        }
    }
}
