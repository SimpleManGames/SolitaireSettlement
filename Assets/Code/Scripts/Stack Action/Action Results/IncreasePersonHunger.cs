using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility.Collections;
using UnityEngine;

namespace SolitaireSettlement
{
    public class IncreasePersonHunger : IStackActionResult
    {
        [field: SerializeField]
        private int IncreaseHungerAmount { get; set; }

        public void OnResult(IEnumerable<Card> relatedCardStack)
        {
            relatedCardStack.Where(c => c.Info.Data.CardType == CardData.ECardType.Person)
                .ForEach(x =>
                {
                    if (x.Info.Data.CardUse is not PersonHungerCardUse hungerCardUse)
                        return;

                    hungerCardUse.ModifyHungerBy(IncreaseHungerAmount);
                    x.Render.UpdateDynamicTextFields(x.Info.Data);
                });
        }
    }
}