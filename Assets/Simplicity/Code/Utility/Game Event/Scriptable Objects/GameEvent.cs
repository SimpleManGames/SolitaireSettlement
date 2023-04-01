using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BountyHunters
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        [Header("Listeners")]
        [HideLabel]
        [ShowInInspector]
        private readonly List<GameEventListener> _listeners = new();

        [Button]
        public void Raise()
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}