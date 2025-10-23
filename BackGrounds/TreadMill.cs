using System.Collections.Generic;
using UnityEngine;

namespace ShootemUp
{
    public class TreadMill : MonoBehaviour
    {
        public static TreadMill Instance;

        ScrollingBackground _scrollingBackground;
        BackgroundLayer[] _backgroundLayers;
        float _mainBackgroundSpeed = 1.0f;
        bool _canProcessBackgroundLayers = true;

        //List<Transform> _backgrounds = new List<Transform>();
        //float _speed = 1.0f;

        [Space]
        [SerializeField]
        float _speedMultiplier = 1.0f;

        public float speedMultiplier { get { return _speedMultiplier; } }
      

        public void Awake()
        {
            //// 자식 트랜스폼 가져오기
            //foreach (Transform child in this.transform)
            //{
            //    _backgrounds.Add(child);
            //}

            Instance = this;
        }

        private void Start()
        {
            ChacheComponents();
        }

        private void Update()
        {
            MoveLayers();
        }

        void ChacheComponents()
        {
            _canProcessBackgroundLayers = true;
            _scrollingBackground = GetComponent<ScrollingBackground>();

            if(_scrollingBackground == null)
            {
                Debug.Log("Treadmill.cs: can not find the scrolling backfround script");

                return;
            }

            _backgroundLayers = _scrollingBackground.BackgroundLayers;

            if(_backgroundLayers == null || _backgroundLayers.Length < 1)
            {
                _canProcessBackgroundLayers= false;
                Debug.Log("Treadmill.cs: your scrolling background script needs to have the first layer");
                return;
            }

            _mainBackgroundSpeed = _backgroundLayers[0].Speed;
        }

        void FindlfAllLayersHaveTreamillTransform()
        {
            foreach (BackgroundLayer bgl in _backgroundLayers)
            {
                if(bgl.TreadmillTransfrom == null)
                {
                    Debug.Log("Treadmill.cs: can't find the treadmill transform.");
                    _canProcessBackgroundLayers = false;
                    return;
                }

                _mainBackgroundSpeed = _backgroundLayers[0].Speed;
                FindlfAllLayersHaveTreamillTransform();
            }
        }

        void MoveLayers()
        {
            if (Level.IsVertical)
            {
                MoveBackGroundsVertically();
            }
            else if (Level.IsHorizontal)
            {
                MoveBackgroundsHorizontally();
            }
        }

        void MoveBackGroundsVertically()
        {
            if (!_canProcessBackgroundLayers)
            {
                return;
            }

            foreach (BackgroundLayer bgl in _backgroundLayers)
            {
                bgl.TreadmillTransfrom.Translate(Vector3.down * bgl.Speed * _speedMultiplier * Time.deltaTime);

                if(bgl.TreadmillTransfrom.position.y < ProjectConstants.ResettingDistance)
                {
                    TransformTools.ResetParentPosition(bgl.TreadmillTransfrom);
                }
            }

            //foreach (Transform bgl in _backgrounds)
            //{
            //    bgl.Translate(Vector3.down * _speed * speedMultiplier * Time.deltaTime);
            //}
        }

        void MoveBackgroundsHorizontally()
        {
            if (!_canProcessBackgroundLayers)
            {
                return;
            }

            foreach (BackgroundLayer bgl in _backgroundLayers)
            {
                bgl.TreadmillTransfrom.Translate(Vector3.left * bgl.Speed * _speedMultiplier * Time.deltaTime);

                if (bgl.TreadmillTransfrom.position.y < ProjectConstants.ResettingDistance)
                {
                    TransformTools.ResetParentPosition(bgl.TreadmillTransfrom);
                }
            }

            //foreach (Transform bgl in _backgrounds)
            //{
            //    bgl.Translate(Vector3.left * _speed * speedMultiplier * Time.deltaTime);
            //}
        }

        public void ChangeSpeedMultiplier(float newMiltiplier)
        {
            _speedMultiplier = newMiltiplier;
        }
    }
}
