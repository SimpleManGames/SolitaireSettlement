using System.Linq;

namespace SolitaireSettlement
{
    public class ExpendPersonHungerAreaReveal : IAreaReveal
    {
        public void OnAreaReveal(Area area)
        {
            var areaRevealedCount = AreaManager.Instance.GeneratedAreaComponents.Count(a => a.Revealed);
            // First one should be free
            if (areaRevealedCount == 0)
                return;

            if (!area.AnyPersonCardOverlapping())
                return;

            // Expend hunger for the first detected person in the area
            var personCard = area.OverlappingPersons.First();
            var hungerCardUse = personCard.Info.Data.OnTurnUpdate
                .Where(c => c is HungerCardImpl)
                .Cast<HungerCardImpl>().First();

            if (hungerCardUse == null)
                return;

            hungerCardUse.ModifyHungerBy(-AreaManager.Instance.AreaDiscoverHungerCost);
            personCard.Render.UpdateDynamicTextFields(personCard.Info.Data);
        }
    }
}