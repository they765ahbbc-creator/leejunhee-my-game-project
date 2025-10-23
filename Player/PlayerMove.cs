using UnityEngine;

namespace ShootemUp
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField]protected float _speed;
        [SerializeField] protected float _maxSpeed;
        [SerializeField] bool isMovementNormalized = true;

        float _playerSpeedMultiplier = 1.0f;
        float _currentSpeed;
        Vector2 _inputDirection;
        Rigidbody2D _rb;

        public float Speed { get { return _speed; } }
        public float CurrentSpeed
        {
            get{ return _currentSpeed * _playerSpeedMultiplier; }
            private set
            {

                if (value > _maxSpeed)
                {
                    _currentSpeed = _maxSpeed;
                }
                else if (value < _speed)
                {
                    _currentSpeed = _speed;
                }
                else
                {
                    _currentSpeed = value;
                }
            }
        }

        public Vector2 Direction { get; private set; }
        public Vector2 CurrentDirection { get; private set; }
        public Vector2 DisplacementInLastFrame { get; private set; }

        private void Awake()
        {
            
            CurrentSpeed = _speed;
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            FindInputDirection();
            FindCurrentDirection();
            KeepInBoundries();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnValidate()
        {
            if(_maxSpeed < _speed)
            {
                _maxSpeed = _speed;
            }
        }

        private void KeepInBoundries()
        {
            if(PlayField.Instance.Boundries == null)
            {
                return;
            }

            if(PlayField.Instance.Boundries.size == Vector2.zero)
            {
                return;
            }

            transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, PlayField.Instance.Boundries.xMin, PlayField.Instance.Boundries.xMax),
                Mathf.Clamp(transform.position.y, PlayField.Instance.Boundries.yMin, PlayField.Instance.Boundries.yMax),
                transform.position.z
            );
        }

        private void FindDesktopControlsDirection()
        {
            float xAxisInput = Input.GetAxis(ProjectConstants.Horizontal);
            float yAxislnput = Input.GetAxis(ProjectConstants.Vertical);

            _inputDirection = new Vector2 (xAxisInput, yAxislnput);
        }

        private void FindInputDirection()
        {
            FindDesktopControlsDirection();

            if (isMovementNormalized)
            {
                Direction = _inputDirection.normalized;
            }
            else
            {
                Direction = _inputDirection;
            }
        }

        private void FindCurrentDirection()
        {
            CurrentDirection = Direction;
        }

        private void Move()
        {
            DisplacementInLastFrame = CurrentDirection * CurrentSpeed * Time.deltaTime;
            _rb.transform.position += new Vector3(DisplacementInLastFrame.x,DisplacementInLastFrame.y,0.0f);
        }
    }
}
