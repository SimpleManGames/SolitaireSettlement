using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Simplicity.GameEvent
{
    public class GameEventListener : MonoBehaviour
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
            response.Invoke();
        }
    }
}