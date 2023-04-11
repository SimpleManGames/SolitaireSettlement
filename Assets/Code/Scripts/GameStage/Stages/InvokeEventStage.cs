using Simplicity.GameEvent;
using UnityEngine;

namespace SolitaireSettlement
{
    public class InvokeEventStage : IGameStage
    {
        [field: SerializeField]
        private GameEvent Event { get; set; }

        public virtual bool HasFinished()
        {
            return true;
        }

        public virtual void ExecuteStageLogic()
        {
            Event.Raise();
        }
    }
}