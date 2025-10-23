using UnityEngine;

namespace ShootemUp
{
    [System.Serializable]
    public class GoCount
    {
        public GameObject _prefab;

        public int _count;

        public GoCount()
        {
            _prefab = null;
            _count = 0;
        }

        public GoCount(GameObject go)
        {
            _prefab = go;
            _count = 1;
        }

        public GoCount(GameObject go, int count)
        {
            _prefab = go;
            _count = count;
        }
    }
}
