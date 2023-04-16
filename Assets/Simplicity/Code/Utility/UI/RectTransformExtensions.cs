using UnityEngine;

namespace Simplicity.UI
{
    public static class RectTransformExtensions
    {
        public static Vector3 RandomPointWithinRectBounds(this RectTransform rect)
        {
            var bounds = rect.Bounds();
            return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
        }

        public static void ClampWithin(this RectTransform a, RectTransform b)
        {
            var aBounds = a.Bounds();
            var bBounds = b.Bounds();

            a.position = new Vector3(
                Mathf.Clamp(a.position.x, bBounds.min.x + aBounds.extents.x, bBounds.max.x - aBounds.extents.x),
                Mathf.Clamp(a.position.y, bBounds.min.y + aBounds.extents.y, bBounds.max.y - aBounds.extents.y));
        }

        public static bool Contains(this RectTransform a, RectTransform b)
        {
            var aBounds = a.WorldRect();
            var bBounds = b.WorldRect();

            return aBounds.Contains(bBounds.min) && aBounds.Contains(bBounds.max);
        }

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

        public static Bounds Bounds(this RectTransform rectTransform)
        {
            return new Bounds(rectTransform.WorldRect().center, rectTransform.WorldRect().size);
        }
    }
}