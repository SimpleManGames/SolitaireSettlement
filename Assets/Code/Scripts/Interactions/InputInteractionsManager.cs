using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolitaireSettlement
{
    public class InputInteractionsManager : MonoBehaviour, GameInputs.IInteractionsActions
    {
        private GameInputs _gameInputs;

        [field: SerializeField]
        private GameAreaInteractionsManager gameAreaInteractionsManager;

        [field: SerializeField]
        private HandAreaInteractionsManager handAreaInteractionsManager;

        [field: Title("Readonly")]
        [field: SerializeField, ReadOnly]
        public Vector2 InteractionPoint { get; private set; }

        [field: ShowInInspector, ReadOnly] public GameObject CurrentDragObject { get; set; }

        [field: ShowInInspector, ReadOnly] public IUIDrag CurrentDraggable { get; set; }

        [field: ShowInInspector, ReadOnly] public GameObject CurrentClickObject { get; set; }

        [field: ShowInInspector, ReadOnly] public IUIClickable CurrentClickable { get; set; }

        private void Awake()
        {
            _gameInputs = new GameInputs();
            _gameInputs.Interactions.SetCallbacks(this);
            _gameInputs.Enable();
        }

        public void OnPress(InputAction.CallbackContext context)
        {
            HandlePress(context);
        }

        public void OnInteractionPoint(InputAction.CallbackContext context)
        {
            InteractionPoint = context.ReadValue<Vector2>();
        }

        private void HandlePress(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    if (HandleInteractionsManager(handAreaInteractionsManager))
                        return;
                    if (HandleInteractionsManager(gameAreaInteractionsManager))
                        return;
                    break;
                case InputActionPhase.Canceled:
                    handAreaInteractionsManager.HandleInteractionResults();
                    gameAreaInteractionsManager.HandleInteractionResults();

                    CurrentClickable?.OnClick();
                    CurrentDraggable?.OnDragEnd();

                    CurrentDragObject = null;
                    CurrentDraggable = null;
                    CurrentClickObject = null;
                    CurrentClickable = null;
                    break;
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                case InputActionPhase.Started:
                default:
                    break;
            }
        }

        private bool HandleInteractionsManager(IInteractionsManager interactionsManager)
        {
            interactionsManager.CastRaysToUI();

            if (interactionsManager.ParseClickResultsForClickable())
                return true;

            if (interactionsManager.ParseClickResultsForDraggable())
                return true;

            return false;
        }
    }
}