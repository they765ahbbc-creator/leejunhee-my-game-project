using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootemUp
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        List<Wave> _waves;

        float _mindRndSpawnPositoin = 0.3f;
        float _mAXdRndSpawnPositoin = 0.7f;

        void Start()
        {
            StartCoroutine(SpawnWave());
        }

        void Update()
        {

        }

        IEnumerator SpawnWave()
        {
            yield return new WaitForEndOfFrame();

            int cnt = 0;
            foreach (Wave wave in _waves)
            {
                float waitNext = wave.WaitNextTime;
                for (int i = 0; i < wave.EnemyCnt; i++)
                {
                    Vector3 spawnPosition = FindSpawnPosition(10, 0, wave.RectSide);
                    yield return spawnPosition;

                    GameObject enemy = Instantiate(wave.Enemy, spawnPosition, Quaternion.identity, this.transform);
                    yield return null;
                    EnemyMove em = enemy.GetComponent<EnemyMove>();
                    if (em != null)
                    {
                        enemy.layer = projectLayers.Enemy;
                        em.RectSide = wave.RectSide;
                        em.SetSpriteAngle();
                        em.EnemyType = wave.EnemyType;
                    }
                    else
                    {
                        enemy.layer = projectLayers.Enemy;
                        enemy.transform.Translate(new Vector3(0, 4, 0));
                    }
                    yield return new WaitForSeconds(wave.DelayTime);
                }
                cnt++;
                yield return new WaitForSeconds(waitNext);
            }

            if(cnt != _waves.Count)
            {
                StartCoroutine(SpawnWave());
            }

        }

        Vector3 FindSpawnPosition(int depthlndex, float offest, RectSide spawnSide)
        {
            Vector3 spawnPosition = Vector3.zero;
            spawnPosition.z = -depthlndex * Level.SpaceBetweenIndices;
            if (spawnSide == RectSide.Left || spawnSide == RectSide.Right)
            {
                _mindRndSpawnPositoin = 0.5f;
                _mAXdRndSpawnPositoin = 0.8f;
            }

            float spawnPositionRatio = Random.Range(_mindRndSpawnPositoin, _mAXdRndSpawnPositoin);

            switch (spawnSide)
            {
                case RectSide.Top:
                    spawnPosition.x = Mathf.Lerp(PlayField.Instance.Boundries.xMin, PlayField.Instance.Boundries.xMax, spawnPositionRatio);
                    spawnPosition.y = PlayField.Instance.Boundries.yMax + offest;
                    break;

                case RectSide.Bottom:
                    spawnPosition.x = Mathf.Lerp(PlayField.Instance.Boundries.xMin, PlayField.Instance.Boundries.xMax, spawnPositionRatio);
                    spawnPosition.y = PlayField.Instance.Boundries.yMin - offest;
                    break;
                case RectSide.Left:
                    spawnPosition.x = PlayField.Instance.Boundries.xMin - offest;
                    spawnPosition.y = Mathf.Lerp(PlayField.Instance.Boundries.yMin, PlayField.Instance.Boundries.yMax, spawnPositionRatio);
                    break;
                case RectSide.Right:
                    spawnPosition.x = PlayField.Instance.Boundries.xMax + offest;
                    spawnPosition.y = Mathf.Lerp(PlayField.Instance.Boundries.yMin, PlayField.Instance.Boundries.yMax, spawnPositionRatio);
                    break;
            }

            return spawnPosition;
        }
    }
}
