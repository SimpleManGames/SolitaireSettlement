using System.Collections.Generic;
using Sirenix.OdinInspector;
using SolitaireSettlement;
using UnityEngine;

namespace Simplicity.GameEvent
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        [Header("Listeners"), HideLabel, ShowInInspector]
        private readonly List<IGameEventListener> _listeners = new();

        [Button]
        public void Raise()
        {
            for (var i = 0; i < _listeners.Count; i++)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(IGameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public override string ToString()
        {
            return $"{name}";
        }
    }
}