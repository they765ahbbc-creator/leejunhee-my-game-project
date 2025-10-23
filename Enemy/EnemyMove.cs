using UnityEngine;

namespace ShootemUp
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] EnemyType _etype;
        [SerializeField] float _verticalSpeed = 2f;
        [SerializeField] float _horizontalSpeed = 2f;
        [SerializeField] float _amplitude = 2f;

        RectSide _rectSide;
        float _startX;
        float _startY;
        float _timeSinceSpawn;

        public EnemyType EnemyType { get { return _etype; } set { _etype = value; } }
        public RectSide RectSide { get { return _rectSide; } set { _rectSide = value; } }

        void Start()
        {
            _startX = transform.position.x;
            _startY = transform.position.y;
            _timeSinceSpawn = 0f;
            SetSpriteAngle();
        }

        void Update()
        {
            switch (_rectSide)
            {
                case RectSide.Left:
                    MoveRight();
                    break;

                case RectSide.Right:
                    MoveLeft();
                    break;

                case RectSide.Top:
                    MoveDown();
                    break;

                case RectSide.Bottom:
                    MoveUp();
                    break;
            }
        }

        void MoveLeft()
        {
            if (_etype == EnemyType.Straight)
            {
                transform.Translate(Vector3.up * _verticalSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                _timeSinceSpawn += Time.deltaTime;

                float newX = transform.position.x - _horizontalSpeed * Time.deltaTime;
                float newY = _startY - Mathf.Sin(_timeSinceSpawn * _verticalSpeed) * _amplitude;

                transform.position = new Vector3(newX, newY, transform.position.z);
            }
        }

        void MoveRight()
        {
            if (_etype == EnemyType.Straight)
            {
                transform.Translate(Vector3.up * _verticalSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                _timeSinceSpawn += Time.deltaTime;

                float newX = transform.position.x + _horizontalSpeed * Time.deltaTime;
                float newY = _startY - Mathf.Sin(_timeSinceSpawn * _verticalSpeed) * _amplitude;

                transform.position = new Vector3(newX, newY, transform.position.z);
            }
        }

        void MoveUp()
        {
            if (_etype == EnemyType.Straight)
            {
                transform.Translate(Vector3.up * _verticalSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                _timeSinceSpawn += Time.deltaTime;

                float newX = _startX + Mathf.Sin(_timeSinceSpawn * _verticalSpeed) * _amplitude;
                float newY = transform.position.y + _horizontalSpeed * Time.deltaTime;

                transform.position = new Vector3(newX, newY, transform.position.z);
            }
        }

        void MoveDown()
        {
            if(_etype == EnemyType.Straight)
            {
                transform.Translate(Vector3.up * _verticalSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                _timeSinceSpawn += Time.deltaTime;

                float newX = _startX + Mathf.Sin(_timeSinceSpawn * _verticalSpeed) * _amplitude;
                float newY = transform.position.y - _horizontalSpeed * Time.deltaTime;

                transform.position = new Vector3(newX, newY, transform.position.z);
            }
        }

        public void SetSpriteAngle()
        {
            float angle = 0f;
            switch (RectSide)
            {
                case RectSide.Top:
                    angle = 90f;
                    break;

                case RectSide.Bottom:
                    angle = -90f;
                    break;

                case RectSide.Left:
                    angle = 180f;
                    break;

                case RectSide.Right:
                    angle = 0f;
                    break;
            }

            transform.Rotate(new Vector3(0, 0, angle));
        }
    }
}
