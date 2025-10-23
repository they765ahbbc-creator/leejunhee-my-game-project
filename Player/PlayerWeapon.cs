using UnityEngine;

namespace ShootemUp
{
    public class PlayerWeapon : Weapon
    {
        float _fireTimer = 0f;

        private void Awake()
        {
            SetWeaponStrategy(WeaponStrat);
        }
        
        void Start()
        {
            WeaponInit();
        }

        void Update()
        {
            _fireTimer += Time.deltaTime;

            if(Input.GetAxis(ProjectConstants.Fire1) >= 0.1 && _fireTimer >= WeaponStrat.FireRate)
            {
                WeaponStrat.Fire(_firePoint, projectLayers.PlayerBullet, WeaponStrat.Damage);
                _fireTimer = 0f;
            }
        }

        void WeaponInit()
        {
            //SingleShot ws = (SingleShot)WeaponStrat;
            MultiShot ws = (MultiShot)WeaponStrat;
            ws.Damage = 10;
            ws.FireRate = 0.6f;
        }
    }
}
