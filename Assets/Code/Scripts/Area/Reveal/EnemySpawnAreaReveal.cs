using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace SolitaireSettlement
{
    public class EnemySpawnAreaReveal : IAreaReveal
    {
        [field: SerializeField, Tooltip("The first amount of X areas are guaranteed safe.")]
        private int InitialSafeAreas { get; set; } = 8;

        [field: SerializeField, Range(0, 100)]
        private int PercentChanceForEnemySpawn { get; set; } = 10;

        [field: SerializeField, AssetsOnly]
        private CardData EnemyBaseCard { get; set; }

        [field: SerializeField, AssetsOnly]
        private CardData EnemyCard { get; set; }

        [field: SerializeField, AssetsOnly]
        private AreaGenerator Generator { get; set; }

        private Random _rng = new();

        public void OnAreaReveal(Area area)
        {
            // Check if we should spawn an enemy camp
            var revealedAreaCount = AreaManager.Instance.GeneratedAreaComponents.Count(a => a.Revealed);

            if (revealedAreaCount < InitialSafeAreas)
                return;

            if (_rng == null)
                _rng = new Random(Generator.Seed);

            var roll = _rng.Next(0, 100);
            if (roll > PercentChanceForEnemySpawn)
                return;

            // Replace/Add enemy camp to area
            area.CardObjectsInArea[0].UpdateCardData(EnemyBaseCard);

            // Spawn an extra enemy right away in area
            CardManager.Instance.CreateNewCardRuntimeInfo(EnemyCard,
                CardRuntimeInfo.CardLocation.GameBoard, false, area.transform.position, 0);
        }
    }
}