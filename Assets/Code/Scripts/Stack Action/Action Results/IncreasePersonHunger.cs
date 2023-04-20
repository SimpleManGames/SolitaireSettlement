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
                    var hungerImplList = x.Info.Data.OnTurnUpdate
                        .Where(w => w is HungerCardImpl)
                        .Cast<HungerCardImpl>().ToList();

                    foreach (var hungerImpl in hungerImplList)
                    {
                        hungerImpl.ModifyHungerBy(IncreaseHungerAmount);
                    }
                });
        }
    }
}