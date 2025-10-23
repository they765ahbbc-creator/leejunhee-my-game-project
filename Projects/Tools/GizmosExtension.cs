using UnityEngine;

namespace ShootemUp
{
    public static class GizmosExtension
    {
        #region Rect

        public static void DrawRect(Rect rect, Color color)
        {
            Gizmos.color = color;
            Vector3 gizmoSize = new Vector3(rect.width, rect.height, 0.0f);
            Gizmos.DrawWireCube(rect.center, gizmoSize);
        }

        public static void DrawRect(Vector2 vector2, Color color)
        {
            Gizmos.color = color;
            Vector3 gizmoSize = new Vector3(vector2.x , vector2.y, 0.0f);
            Gizmos.DrawWireCube(Vector3.zero, gizmoSize);
        }

        public static void DrawRect(Vector2 vector2, Vector2 center, Color color)
        {
            Gizmos.color = color;
            Vector3 gizmoCenter = new Vector3(center.x, center.y, 0.0f);
            Vector3 gizmoSize = new Vector3(vector2.x, vector2.y, 0.0f);
            Gizmos.DrawWireCube(gizmoCenter, gizmoSize);
        }

        #endregion

        #region Circle

        public static void DrawCircle(Vector2 center, float radius, float depth, float deltaAngle)
        {
            int SegmentNum = Mathf.CeilToInt(360f / deltaAngle);
            float RadialDegree = deltaAngle * Mathf.Deg2Rad;
            Vector3 Center = new Vector3(center.x, center.y, 0);

            for(int i = 0; i < SegmentNum; i++)
            {
                Vector3 From;
                Vector3 To;
                if(i == SegmentNum - 1)
                {
                    From = new Vector3(Mathf.Cos(RadialDegree * i) * radius, Mathf.Sin(RadialDegree * i) * radius,depth) + Center;
                    To = new Vector3(radius, 0, depth) + Center;
                }
                else
                {
                    From = new Vector3(Mathf.Cos(RadialDegree * i) * radius, Mathf.Sin(RadialDegree * i) *radius,depth) + Center;
                    To = new Vector3(Mathf.Cos(RadialDegree * (i + 1)) * radius, Mathf.Sin(RadialDegree * (i - 1)) * radius, depth) + Center;
                }
                Gizmos.DrawLine(From, To);
            }
        }

        public static void DrawCircle(Vector2 center, float radius)
        {
            DrawCircle(center, radius, 0, 3f);
        }

        public static void DrawCircle (Vector2 center, float radius, float depth)
        {
            DrawCircle(center, radius, depth, 3f);
        }

        #endregion
    }
}
