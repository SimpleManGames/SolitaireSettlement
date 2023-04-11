using Simplicity.GameEvent;
using UnityEngine;

namespace SolitaireSettlement
{
    public class PlanningStage : IGameStage, IGameEventListener
    {
        [field: SerializeField]
        private GameEvent OnPlanningFinished { get; set; }

        private bool _planningFinished;

        public void Setup()
        {
            OnPlanningFinished.RegisterListener(this);
        }

        public bool HasFinished()
        {
            return _planningFinished;
        }

        public void ExecuteStageLogic()
        {
            _planningFinished = false;
        }

        public void OnEventRaised()
        {
            _planningFinished = true;
        }

        public override string ToString()
        {
            return "PlanningStage";
        }
    }
}