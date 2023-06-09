using DG.Tweening;
using Simplicity.GameEvent;
using UnityEngine;

namespace SolitaireSettlement
{
    public class InvokeEventStage : IGameStage
    {
        [field: SerializeField]
        private GameEvent Event { get; set; }

        [field: SerializeField]
        private float DelayBeforeNextStage { get; set; } = 1.0f;

        private float _currentDelay = 0.0f;

        public void Setup()
        {
            _currentDelay = 0.0f;
        }

        public virtual bool HasFinished()
        {
            var finished = DOTween.TotalTweensById("Card", true) == 0;
            if (finished)
                _currentDelay += Time.deltaTime;

            return finished && _currentDelay > DelayBeforeNextStage;
        }

        public virtual void ExecuteStageLogic()
        {
            _currentDelay = 0.0f;
            Event.Raise();
        }

        public override string ToString()
        {
            return $"InvokeEventStage Event:{Event}";
        }
    }
}