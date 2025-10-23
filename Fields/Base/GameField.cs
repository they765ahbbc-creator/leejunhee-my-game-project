using UnityEditor;
using UnityEngine;

namespace ShootemUp
{
    public abstract class GameField : MonoBehaviour
    {
        [SerializeField]
        protected FieldSource _fieldSource;

        [SerializeField]
        protected float _offset;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        protected float _offsetTop;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        protected float _offsetBottom;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        protected float _offsetLeft;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        protected float _offsetRight;

#if UNITY_EDITOR
        [Header("Gizmos Settings")]
        [SerializeField]
        protected bool isDrawingField;
#endif
        protected Rect boundries;

        protected abstract Color GizmosColor { get; }

        protected abstract void Initialize();

        protected virtual void SetBoundriesToRect(Rect rect)
        {
            boundries = new Rect
            {
                xMin = rect.xMin + rect.width * _offsetLeft,
                xMax = rect.xMin + rect.width * (1.0f - _offsetRight),
                yMin = rect.yMin + rect.height * _offsetBottom,
                yMax = rect.yMin + rect.height * (1.0f - _offsetTop)
            };

            boundries.size += (Vector2.one * _offset * 2.0f);
            boundries.center -= (Vector2.one * _offset);
        }

        protected void FindFielBoundries()
        {
            switch (_fieldSource)
            {
                case FieldSource.Viewfield:
                    {
                        if (LevelCamera.Instance == null)
                        {
                            return;
                        }

                        Rect viewfieldRect = LevelCamera.Instance.Viewfield;
                        SetBoundriesToRect(viewfieldRect);
                        break;
                    }

                case FieldSource.BackgroundDimensions:
                    {
                        if (ScrollingBackground.Instance == null)
                        {
                            return;
                        }

                        Rect mainBackgroundPrefabRect = Dimensions.Vector2ToRect(ScrollingBackground.Instance.MainBackgroundPrefabDimensoins);
                        SetBoundriesToRect(mainBackgroundPrefabRect);
                        break;
                    }
            }
        }

        protected virtual void AddRigidBody2D()
        {
            Rigidbody2D rb2D = gameObject.AddComponent<Rigidbody2D>();
            rb2D.bodyType = RigidbodyType2D.Kinematic;
        }

        protected virtual void AddBoxCollider2D()
        {
            BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
            boxCollider2D.size = boundries.size;
            boxCollider2D.offset = boundries.center;
        }

#if UNITY_EDITOR
        public void FindBoundriesInEditor()
        {
            switch (_fieldSource)
            {
                case FieldSource.Viewfield:
                    {
                        LevelCamera levelCamera = FindFirstObjectByType<LevelCamera>();

                        if (levelCamera == null)
                        {
                            return;
                        }

                        levelCamera.Initalize();

                        Rect viewfieldRect = levelCamera.Viewfield;
                        SetBoundriesToRect(viewfieldRect);
                        break;
                    }

                case FieldSource.BackgroundDimensions:
                    {
                        ScrollingBackground scrollingBackground = FindFirstObjectByType<ScrollingBackground>();

                        if (scrollingBackground == null)
                        {
                            return;
                        }

                        scrollingBackground.InitBackgroundDimensons();
                        Rect mainBackgroundPrefabRect = Dimensions.Vector2ToRect(scrollingBackground.MainBackgroundPrefabDimensoins);
                        SetBoundriesToRect(mainBackgroundPrefabRect);
                        break;
                    }
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (isDrawingField)
            {
                if (!EditorApplication.isPlaying)
                {
                    FindBoundriesInEditor();
                }

                GizmosExtension.DrawRect(boundries, GizmosColor);

                GUIStyle guiStyle = new GUIStyle();
                guiStyle.normal.textColor = GizmosColor;

                string className = StringTools.FindClassName(GetType());

                Handles.Label(GetGizmoLabelPosition(), className, guiStyle);
            }
        }

        protected abstract Vector2 GetGizmoLabelPosition();
#endif
    } 
}
