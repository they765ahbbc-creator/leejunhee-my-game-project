using UnityEngine;

namespace ShootemUp
{
    public sealed class Level : MonoBehaviour
    {
        public static Level Instance;

        public static bool IsVertical;
        public static bool IsHorizontal;

        [Header("Level")]
        [SerializeField]
        LevelType _levelType;

        [SerializeField]
        float _spaceBetweem2DDepthIndices = ProjectConstants.DefaultSpaceBetween2DDepthlndices;

        public float SpaceBetween2DDepthIndices
        {
            get { return _spaceBetweem2DDepthIndices; }
        }

        public static float SpaceBetweenIndices { get; set; }

        private void Awake()
        {
            Instance = this; //Singleton Pattern
            DetermineLevelType();
        }

        private void OnValidate()
        {
            DetermineLevelType();
            ValidateSpaceBetween2DDepthIndices();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void DetermineLevelType()
        {
            switch (_levelType)
            {
                case LevelType.VerticalScrolling2D:
                    IsVertical = true;
                    IsHorizontal = false; 
                    break;

                case LevelType.HorizontalScrolling2D:
                    IsVertical = false;
                    IsHorizontal = true; 
                    break;

                default:
                    IsVertical = true;
                    IsHorizontal = false;
                    break;

            }
        }

        private void ValidateSpaceBetween2DDepthIndices()
        {
            if (_spaceBetweem2DDepthIndices <= 0.0f)
            {
                _spaceBetweem2DDepthIndices = 1.0f;
            }
        }
    }
}
