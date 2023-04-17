using System;
using Cinemachine;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolitaireSettlement
{
    public class CameraManager : Singleton<CameraManager>, GameInputs.ICameraControlsActions
    {
        [field: SerializeField]
        public Camera MainCamera { get; private set; }

        [field: SerializeField]
        private CinemachineVirtualCamera VirtualCamera { get; set; }

        [field: SerializeField]
        public Canvas ScreenSpaceCanvas { get; private set; }

        [field: SerializeField]
        public RectTransform HandContainer { get; private set; }

        [field: SerializeField]
        public Canvas GameAreaCanvas { get; private set; }

        [field: Title("Zoom Settings")]
        [field: SerializeField]
        private float ZoomSpeed { get; set; } = 2.0f;

        [field: SerializeField, MinMaxSlider(0, 100, true)]
        private Vector2 ZoomInMinMax { get; set; }

        [field: Title("Pan Settings")]
        [field: ShowInInspector, ReadOnly]
        private bool Panning { get; set; } = false;

        [field: SerializeField]
        private float StartingPanSpeed { get; set; } = 2.0f;

        [field: SerializeField]
        private float MaxPanSpeed { get; set; } = 2.0f;

        [field: SerializeField]
        private float TimeToReachMaxPanSpeed { get; set; } = 2.0f;

        [field: SerializeField]
        private AnimationCurve PanSpeedRampUpCurve { get; set; }

        private float _currentPanningTime = 0.0f;

        private Vector2 _panDirection;

        [field: Title("Drag Settings")]
        [field: SerializeField]
        private float DragSpeed { get; set; }

        private bool Dragging { get; set; }

        private Vector3 _dragOrigin;

        private Vector2 _mousePosition;

        [ShowInInspector]
        private Vector3 WorldMousePosition => MainCamera.ScreenToWorldPoint(
            new Vector3(_mousePosition.x, _mousePosition.y, -MainCamera.transform.position.z));

        private GameInputs _gameInputs;

        public override void Awake()
        {
            base.Awake();

            _gameInputs = new GameInputs();
            _gameInputs.CameraControls.SetCallbacks(this);
            _gameInputs.CameraControls.Enable();

            MainCamera = VirtualCamera.VirtualCameraGameObject.GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (Panning && !Dragging)
            {
                _currentPanningTime += Time.deltaTime;

                var normalizedPanTime = _currentPanningTime / TimeToReachMaxPanSpeed;
                var pointAlongCurve = PanSpeedRampUpCurve.Evaluate(normalizedPanTime);
                var panSpeed = Mathf.Lerp(StartingPanSpeed, MaxPanSpeed, pointAlongCurve);

                var position = MainCamera.transform.position;
                MainCamera.transform.position =
                    Vector3.Lerp(position, position + (Vector3)_panDirection, panSpeed * Time.deltaTime);
            }
            else
            {
                _currentPanningTime = 0.0f;
            }

            if (Dragging)
            {
                var cameraPosition = MainCamera.transform.position;
                var diff = WorldMousePosition - cameraPosition;
                MainCamera.transform.position = _dragOrigin - diff;
            }
        }

        public bool GetPositionOnGameAreaCanvas(Vector2 screenPoint, out Vector2 position)
        {
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)GameAreaCanvas.transform, screenPoint, MainCamera, out position);
        }

        public void GetPositionOnScreenSpaceCanvas(Vector3 worldPoint, out Vector2 position)
        {
            position = Instance.MainCamera.WorldToScreenPoint(worldPoint);
        }

        public void OnPan(InputAction.CallbackContext context)
        {
            _mousePosition = context.ReadValue<Vector2>();

            _panDirection = new Vector2();
            if (_mousePosition.y >= Screen.height * 0.95f)
                _panDirection.y += 1;
            else if (_mousePosition.y <= Screen.height * 0.05f)
                _panDirection.y -= 1;

            if (_mousePosition.x >= Screen.width * 0.95f)
                _panDirection.x += 1;
            else if (_mousePosition.x <= Screen.width * 0.05f)
                _panDirection.x -= 1;

            Panning = _panDirection != Vector2.zero;
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            var increment = context.ReadValue<float>();

            var fov = VirtualCamera.m_Lens.FieldOfView;
            var target = Mathf.Clamp(fov - increment, ZoomInMinMax.x, ZoomInMinMax.y);
            VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, ZoomSpeed * Time.deltaTime);
        }

        public void OnDrag(InputAction.CallbackContext context)
        {
            Dragging = context.started || context.performed;
            if (context.started)
                _dragOrigin = WorldMousePosition;
        }
    }
}