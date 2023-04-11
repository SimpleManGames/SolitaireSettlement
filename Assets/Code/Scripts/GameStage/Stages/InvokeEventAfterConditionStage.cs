using UnityEngine;

namespace SolitaireSettlement
{
    public class InvokeEventAfterConditionStage : InvokeEventStage
    {
        [field: SerializeField]
        private IStageConditionCheck Condition { get; set; }

        public override bool HasFinished()
        {
            return true;
        }

        public override void ExecuteStageLogic()
        {
            if (Condition.CheckCondition())
                base.ExecuteStageLogic();
        }
    }
}