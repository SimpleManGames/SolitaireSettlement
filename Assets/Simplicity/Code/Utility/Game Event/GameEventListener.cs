using System;
using Sirenix.OdinInspector;
using SolitaireSettlement;
using UnityEngine;
using UnityEngine.Events;

namespace Simplicity.GameEvent
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {
        [HorizontalGroup("Event")]
        [SerializeField] private GameEvent gameEvent;

        [SerializeField] private UnityEvent response;

#if UNITY_EDITOR
        [HorizontalGroup("Event")]
        [Button(ButtonStyle.CompactBox, Name = "Raise")]
        private void RaiseGameEvent()
        {
            gameEvent.Raise();
        }
#endif

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            try
            {
                response.Invoke();
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("GameEventListener Response could not be Invoked due to Null Reference!\n" +
                               $"GameObject: {gameObject.name}\n" +
                               $"Full Message: {e}");
            }
        }
    }
}