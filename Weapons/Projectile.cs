using UnityEngine;


namespace ShootemUp
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] GameObject _hit;
        [SerializeField] GameObject _flash;
        [SerializeField] float _hitOffset = 0f;

        Rigidbody2D _rb;
        int _damage;

        public float Speed { set { _speed = value; } }
        public int Damage { get { return _damage; } set { _damage = value; } }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            if(_flash != null)
            {
                var muzzleVFX = Instantiate(_flash, transform.position, Quaternion.identity);
                muzzleVFX.transform.forward = gameObject.transform.forward;

                DestroyParticleSystem(muzzleVFX);
            }
            transform.SetParent(null);
        }

        void FixedUpdate()
        {
            if(_speed != 0)
            {
                _rb.linearVelocity = transform.forward * _speed;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _speed = 0;

            ContactPoint2D contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * _hitOffset;

            if (_hit != null)
            {
                var hitInstance = Instantiate(_hit, pos, rot);
                hitInstance.transform.LookAt(contact.point + contact.normal);

                DestroyParticleSystem(hitInstance);
            }


            Destroy(gameObject);
        }

        void DestroyParticleSystem(GameObject vfx)
        {
            var ps = vfx.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                ps = vfx.GetComponentInChildren<ParticleSystem>();
            }
            Destroy(vfx, ps.main.duration);
        }
    }
}
