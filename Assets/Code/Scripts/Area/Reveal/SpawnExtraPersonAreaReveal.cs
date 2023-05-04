using System.Linq;

namespace SolitaireSettlement
{
    public class SpawnExtraPersonAreaReveal : IAreaReveal
    {
        public void OnAreaReveal(Area area)
        {
            var personCount = CardManager.Instance.PersonCount;

            if (personCount <= 0)
                return;

            if (!CardManager.Instance.CanAddToPopulation)
                return;

            var areaCount = AreaManager.Instance.GeneratedAreaComponents.Count(a => a.Revealed);

            if (areaCount <= 0)
                return;

            if (areaCount / personCount < AreaManager.Instance.AreaPersonRatio)
                return;

            if (area.Data == AreaManager.Instance.EvilAreaData)
                return;

            CardManager.Instance.CreateNewCardRuntimeInfo(CardManager.Instance.PersonCard,
                CardRuntimeInfo.CardLocation.GameBoard, false, area.transform.position, 0);
        }
    }
}