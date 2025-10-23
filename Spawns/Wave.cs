using UnityEngine;

namespace ShootemUp
{
    [System.Serializable]
    public class Wave
    {
        [SerializeField] EnemyType _enemyType;
        [SerializeField] RectSide _side;
        [SerializeField] GameObject _enemy;
        [SerializeField] int _enemyCount = 1;
        [SerializeField] float _delayTime = 2.0f;
        [SerializeField] float _waitNextWave = 3.0f;

        public EnemyType EnemyType { get { return _enemyType; } set { _enemyType = value; } }
        public RectSide RectSide { get { return _side; } set { _side = value; } }
        public GameObject Enemy { get { return _enemy; } }
        public int EnemyCnt { get { return _enemyCount; } }
        public float DelayTime { get { return _delayTime; } }
        public float WaitNextTime { get { return _waitNextWave; }}
    }
}
