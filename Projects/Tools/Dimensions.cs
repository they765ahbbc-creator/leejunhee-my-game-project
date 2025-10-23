using UnityEngine;

namespace ShootemUp
{
    public static class Dimensions
    {
        public static float FindWidth(GameObject go)
        {
            if(go.GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                return spriteRenderer.bounds.size.x;
            }
            else if (go.GetComponent<MeshRenderer>() != null)
            {
                MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
                return meshRenderer.bounds.size.x;  
            }
            else
            {
                return CannotFindSize(go);
            }
        }

        public static float FindHeight(GameObject go)
        {
            if (go.GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                return spriteRenderer.bounds.size.y;
            }
            else if (go.GetComponent<MeshRenderer>() != null)
            {
                MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
                return meshRenderer.bounds.size.y;
            }
            else
            {
                return CannotFindSize(go);
            }
        }

        private static float CannotFindSize(GameObject go)
        {
            Debug.LogWarning("Dimension.cs: The FindWidth/Height method cannot function.");

            return 0.0f;
        }

        public static Rect Vector2ToRect(Vector2 size)
        {
            Vector2 rectBottomLeftCorner = new Vector2((size.x * -0.5f), (size.y * -0.5f));
            Rect rect = new Rect(rectBottomLeftCorner, size);

            return rect;
        }

        public static float FindZpositionInPlayField(int depthIndex)
        {
            return (-depthIndex * Level.SpaceBetweenIndices);
        }

        public static float VerticalAspectRatioToFloat(VerticalAspectRatio aspect, float inputVerticalAspectRatio)
        {
            switch (aspect)
            {
                case VerticalAspectRatio.Aspect3By4:
                    return 3.0f / 4.0f;

                case VerticalAspectRatio.Aspect4By5:
                    return 4.0f / 5.0f;

                case VerticalAspectRatio.Aspect9By16:
                    return 9.0f / 16.0f;

                case VerticalAspectRatio.Aspect10By16:
                    return 10.0f / 16.0f;

                case VerticalAspectRatio.Input:
                    return inputVerticalAspectRatio;

                default:
                    return 1.0f;
            }
        }
    }
}
