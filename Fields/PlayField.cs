using UnityEngine;

namespace ShootemUp
{
    public class PlayField : GameField
    {
        public static PlayField _instance;

        public static PlayField Instance { get { return _instance; } set { _instance = value; } }

        public Rect Boundries
        {
            get
            {
                return boundries;
            }
        }



        protected override Color GizmosColor { get { return Color.yellow; } }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            FindFielBoundries();
            AddRigidBody2D();
            AddBoxCollider2D();
        }

#if UNITY_EDITOR
        protected override Vector2 GetGizmoLabelPosition()
        {
            return new Vector2(boundries.xMin + ProjectConstants.LabelMarginDistance, boundries.yMin);
        }
#endif
    }
}
