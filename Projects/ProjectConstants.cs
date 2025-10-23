using UnityEngine;

namespace ShootemUp
{
    public static class ProjectConstants
    {
        public const int DepthIndexLimit = 99;

        public const float DefaultSpaceBetween2DDepthlndices = 1.0f;

        public const string LayerPrefix = "Layer";

        public static int LayerPrefixLength
        {
            get
            {
                return LayerPrefix.Length + 2;
            }
        }

        public const int MinActiveBackgroundsLimit = 3;

        public const float ResettingDistance = -200f;

        public const float LabelMarginDistance = 0.5f;

        public const string Vertical = "Vertical";
        public const string Horizontal = "Horizontal";
        public const string Fire1 = "Fire1";
        public const string Fire2 = "Fire2";
    }
}
