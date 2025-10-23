using UnityEngine;

namespace ShootemUp
{
    [RequireComponent(typeof(Camera))]
    public class LevelCamera : MonoBehaviour
    {
        public static LevelCamera Instance;

        public Rect Viewfield { get; private set; }

        [Space]
        [Header("Camera Zoom")]
        [SerializeField]
        private float _zoomFactor = 1.0f;

        [Space]
        [Header("IF Desktop Build & Vertical Level")]
        [SerializeField]
        private VerticalAspectRatio _verticalAspectRatio;
        public VerticalAspectRatio VerticalAspectRatio
        {
            get { return _verticalAspectRatio; }
        }

        [SerializeField] private float _inputVerticalAspectRatio = 1.0f;

        public float VertivalAspect
        {
            get { return Dimensions.VerticalAspectRatioToFloat(_verticalAspectRatio, _inputVerticalAspectRatio); }
        }

        private float _originalCameraAspect;

        public float ZpositionIn2DLevel
        {
            get
            {
                if (Level.Instance != null)
                {
                    return -Level.Instance.SpaceBetween2DDepthIndices * ProjectConstants.DepthIndexLimit;
                }
                else
                {
                    return -ProjectConstants.DefaultSpaceBetween2DDepthlndices * ProjectConstants.DepthIndexLimit;
                }
            }
        }

        float _camera0rthographicSize;
        Camera _cam;
        ScrollingBackground _scrollingBackground;

        private void Awake()
        {
            Instance = this;

            Initalize();
        }

        private void OnValidate()
        {
            if (_zoomFactor <= Mathf.Epsilon)
            {
                _zoomFactor = 1.0f;
            }

            if (_inputVerticalAspectRatio <= Mathf.Epsilon)
            {
                _inputVerticalAspectRatio = 1.0f;
            }

            //Initalize();
        }

        public void Initalize()
        {
            CacheComponents();

            if(_scrollingBackground == null)
            {
                return;
            }

            SetCameraType();
            SetAspectRatio();
            FitCameraSize();
            FindSpaceBetweenIndices();
            ShiftCameraPosition();
            FindCompleteCameraViewfield();
        }

        private void CacheComponents()
        {
            _cam = GetComponent<Camera>();

            _cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            _originalCameraAspect = _cam.aspect;

            _scrollingBackground = ScrollingBackground.Instance;

            if (_scrollingBackground == null)
            {
                _scrollingBackground = FindFirstObjectByType<ScrollingBackground>();
            }
        }

        private void SetCameraType()
        {
            _cam.orthographic = true;
        }

        private void SetAspectRatio()
        {
#if !UNITY_IOS && !UNITY_ANDROID
            if (Level.IsVertical && _verticalAspectRatio != VerticalAspectRatio.None)
            {
                ForceVertical(_verticalAspectRatio);
            }




#else
            if (Level.IsVertical)
            {
                Screen.orientation = ScreenOrientation.Portrait;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
            }

            if (Level.IsHorizontal)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }
#endif
        }

        private void ForceVertical(VerticalAspectRatio aspect)
        {
            float originalToNewAspctRatio = (1.0f / _originalCameraAspect) * VertivalAspect;
            Rect cameraRect = _cam.rect;
            cameraRect.width = originalToNewAspctRatio;
            cameraRect.position = new Vector2(0.5f - (cameraRect.width * 0.5f), 0.0f);
            _cam.rect = cameraRect;
        }

        private void FitCameraSize()
        {
            if(Level.IsVertical)
            {
               _camera0rthographicSize=(_scrollingBackground.MainBackgroundPrefabWidth * 0.5f) / _cam.aspect;
            }
            else if (Level.IsHorizontal)
            {
                _camera0rthographicSize = _scrollingBackground.MainBackgroundPrefabHeight * 0.5f;
            }

            _cam.orthographicSize = _camera0rthographicSize / _zoomFactor;
            
        }

        private void FindSpaceBetweenIndices()
        {
            Level level = Level.Instance;

            if(level == null)
            {
                level = FindFirstObjectByType<Level>();
            }

            if(level == null)
            {
                return;
            }

            Level.SpaceBetweenIndices = level.SpaceBetween2DDepthIndices;
        }

        private void ShiftCameraPosition()
        {
            if (Level.IsVertical)
            {
                float cameraYPosition = _camera0rthographicSize - (_scrollingBackground.MainBackgroundPrefabHeight * 0.5f);
                transform.position = new Vector3(0f, cameraYPosition, ZpositionIn2DLevel);
            }
            else if (Level.IsHorizontal)
            {
                float cameraXPosition = (_camera0rthographicSize * _cam.aspect) - (_scrollingBackground.MainBackgroundPrefabWidth * 0.5f);
                transform.position = new Vector3(cameraXPosition, 0f, ZpositionIn2DLevel);
            }
        }

        private void FindCompleteCameraViewfield()
        {
            Viewfield = new Rect(0f, 0f, ((_camera0rthographicSize * 2) * _cam.aspect), (_camera0rthographicSize * 2)){center = transform.position};
        }

    }
}
