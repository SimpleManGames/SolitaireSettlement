using System.Linq;
using UnityEngine;

namespace SolitaireSettlement
{
    public class PersonCardHealthImpl : CardHealthImpl
    {
        [field: SerializeField]
        private int HealthLostWhileStarving { get; set; }

        private bool _previouslyZeroHunger = false;

        public override void Initialize(Card cardObject)
        {
            base.Initialize(cardObject);

            _previouslyZeroHunger = false;
        }

        public override void TurnProgress(Card card)
        {
            var hungerImpl = card.Info.Data.OnTurnUpdate.First(w => w is HungerCardImpl) as HungerCardImpl;
            if (hungerImpl == null)
            {
                base.TurnProgress(card);
                return;
            }

            if (_previouslyZeroHunger && hungerImpl.CurrentHunger <= 0)
                ModifyHealth(-HealthLostWhileStarving);
            else if (hungerImpl.CurrentHunger <= 0)
                _previouslyZeroHunger = true;
            else
                _previouslyZeroHunger = false;

            base.TurnProgress(card);
        }
    }
}