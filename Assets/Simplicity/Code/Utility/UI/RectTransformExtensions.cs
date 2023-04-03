using UnityEngine;

namespace Simplicity.UI
{
    public static class RectTransformExtensions
    {
        public static bool Overlaps(this RectTransform a, RectTransform b)
        {
            return a.WorldRect().Overlaps(b.WorldRect());
        }

        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
        {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }

        public static Rect WorldRect(this RectTransform rectTransform)
        {
            var sizeDelta = rectTransform.sizeDelta;
            var rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            var rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            var position = rectTransform.position;
            return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f,
                rectTransformWidth, rectTransformHeight);
        }
    }
}