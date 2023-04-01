using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SolitaireSettlement
{
    public class InteractionsManager : MonoBehaviour, GameInputs.IInteractionsActions
    {
        [field: Title("References")]
        [field: SerializeField] private Camera GameCamera { get; set; }

        [field: SerializeField] private Canvas InteractableUICanvas { get; set; }

        [field: Title("Settings")]
        [field: SerializeField] private LayerMask InteractableLayerMask { get; set; }

        [field: Title("Readonly")]
        [field: SerializeField, ReadOnly]
        private Vector2 InteractionPoint { get; set; }

        [field: SerializeField, ReadOnly]
        private Vector3 InteractionWorldPoint { get; set; }

        private GameInputs _gameInputs;

        private GraphicRaycaster _uiRaycaster;
        private PointerEventData _clickData;
        private List<RaycastResult> _clickResults;

        private GameObject _currentDragObject;
        private IUIDraggable _currentDraggable;

        private GameObject _currentClickObject;
        private IUIClickable _currentClickable;

        private void Awake()
        {
            _gameInputs = new GameInputs();
            _gameInputs.Interactions.SetCallbacks(this);
            _gameInputs.Enable();

            _uiRaycaster = InteractableUICanvas.GetComponent<GraphicRaycaster>();
            _clickData = new PointerEventData(EventSystem.current);
            _clickResults = new List<RaycastResult>();
        }

        private void Update()
        {
            if (_currentDragObject == null || _currentDraggable == null)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)InteractableUICanvas.transform, InteractionPoint, GameCamera, out var position);
            _currentDraggable.OnDrag(position);
        }

        public void OnPress(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    _clickData.position = InteractionPoint;
                    _clickResults.Clear();

                    _uiRaycaster.Raycast(_clickData, _clickResults);

                    if (ParseClickResultsForClickable())
                        return;

                    if (ParseClickResultsForDraggable())
                        return;

                    break;
                case InputActionPhase.Canceled:
                    _currentDragObject = null;
                    _currentDraggable = null;
                    _currentClickObject = null;
                    _currentClickable = null;
                    break;
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                case InputActionPhase.Started:
                default:
                    break;
            }
        }

        private bool ParseClickResultsForClickable()
        {
            foreach (var result in _clickResults)
            {
                var resultGameObject = result.gameObject;
                if (resultGameObject.GetComponent<IUIClickable>() == null)
                    continue;

                _currentClickObject = resultGameObject;
                _currentClickable = resultGameObject.GetComponent<IUIClickable>();
                return true;
            }

            return false;
        }

        private bool ParseClickResultsForDraggable()
        {
            foreach (var result in _clickResults)
            {
                var resultGameObject = result.gameObject;
                if (resultGameObject.GetComponent<IUIDraggable>() == null)
                    continue;

                _currentDragObject = resultGameObject;
                _currentDraggable = resultGameObject.GetComponent<IUIDraggable>();
                return true;
            }

            return false;
        }

        public void OnInteractionPoint(InputAction.CallbackContext context)
        {
            InteractionPoint = context.ReadValue<Vector2>();
            var interactionPointVec3 = new Vector3(InteractionPoint.x, InteractionPoint.y, 0.0f);
            InteractionWorldPoint = GameCamera.ScreenToWorldPoint(interactionPointVec3);
        }
    }
}