using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

namespace SolitaireSettlement
{
    public class AddCardFromAreaBonusResult : AddCardToResult
    {
        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var firstCard = relatedCardStack.ElementAt(0);

            var area = firstCard.Area;
            if (area == null)
                return;

            var bonuses = area.CardObjectsInArea
                .Where(c => !c.Info.Data.CardBonuses.IsNullOrEmpty())
                .SelectMany(s => s.Info.Data.CardBonuses).ToList();

            var productionBonuses = bonuses
                .Where(b => b is AreaProductionCardBonus)
                .Cast<AreaProductionCardBonus>().ToList();

            if (productionBonuses.Any(p => p.EffectedCardType == ProducedCard.CardType))
                switch (ProducedCard.CardType)
                {
                    case CardData.ECardType.Food:
                        HandleFoodProductionBonuses(productionBonuses, firstCard);
                        return;
                }
        }

        private void HandleFoodProductionBonuses(List<AreaProductionCardBonus> productionBonuses, Card firstCard)
        {
            var foodProductionBonus = productionBonuses
                .Where(p => p is AreaFoodProductionCardBonus)
                .Cast<AreaFoodProductionCardBonus>().ToList();

            var bonusCount = foodProductionBonus.Sum(f => f.BonusToFoodProductionWithinArea);

            for (var i = 0; i < bonusCount; i++)
                CreateCardTo(ProducedCard, Location, firstCard.transform.position);
        }
    }
}