using UnityEngine;

namespace ShootemUp
{
    public class DespawningField : GameField
    {
        protected override Color GizmosColor
        {
            get { return Color.magenta; }
        }

        private void Awake()
        {
            gameObject.layer = projectLayers.DespawningField;
        }

        private void Start()
        {
            Initialize();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.activeSelf)
            {
                return;
            }

            int collisionLayer = collision.gameObject.layer;

            switch (collisionLayer)
            {
                case projectLayers.Backgrounds:
                    DespawnBackgrounds(collision);
                    break;
                case projectLayers.Enemy:
                    DespawnObject(collision);
                    break;
                case projectLayers.EnemyBullet:
                    DespawnObject(collision);
                    break;
                case projectLayers.PlayerBullet:
                    DespawnObject(collision);
                    break;
            }
        }

        protected override void Initialize()
        {
            AddRigidBody2D();
            FindFielBoundries();
            AddBoxCollider2D();
        }

        private  void DespawnBackgrounds(Collider2D collision)
        {
            if(ScrollingBackground.Instance != null)
            {
                ScrollingBackground.Instance.ReplaceBackground(collision.gameObject);
            }
        }

        private void DespawnObject(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }

#if UNITY_EDITOR
        protected override Vector2 GetGizmoLabelPosition()
        {
            return new Vector2(boundries.xMin + ProjectConstants.LabelMarginDistance, boundries.yMin);
        }
#endif
    }
}
