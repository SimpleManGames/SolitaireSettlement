using Simplicity.Singleton;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CameraManager : Singleton<CameraManager>
    {
        [field: SerializeField]
        public Camera MainCamera { get; private set; }

        [field: SerializeField]
        public Canvas ScreenSpaceCanvas { get; private set; }

        [field: SerializeField]
        public RectTransform HandContainer { get; private set; }

        [field: SerializeField]
        public Canvas GameAreaCanvas { get; private set; }

        public bool GetPositionOnGameAreaCanvas(Vector2 screenPoint, out Vector2 position)
        {
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)GameAreaCanvas.transform, screenPoint, MainCamera, out position);
        }

        public void GetPositionOnScreenSpaceCanvas(Vector3 worldPoint, out Vector2 position)
        {
            position = Instance.MainCamera.WorldToScreenPoint(worldPoint);
        }
    }
}