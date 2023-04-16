using Simplicity.GameEvent;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [RequireComponent(typeof(AreaGenerator))]
    public class AreaManager : Singleton<AreaManager>
    {
        [field: SerializeField]
        private GameEvent OnAreaRevealedEvent { get; set; }

        [field: SerializeField]
        private GameObject GameAreaCanvas { get; set; }

        [field: SerializeField, AssetsOnly]
        private GameObject AreaPrefab { get; set; }

        [ShowInInspector, ReadOnly, HideIf("@this.AreaPrefab == null")]
        private Vector2 _areaSize;

        [field: SerializeField]
        private Vector2 AreaBuffer { get; set; }

        private AreaGenerator _areaGenerator;

        private AreaData[] _generatedAreaData;

        private void Start()
        {
            _areaGenerator = GetComponent<AreaGenerator>();
            _generatedAreaData = _areaGenerator.GenerateRegions(_areaGenerator.GenerateNoise());
            SpawnGeneratedAreas();
        }

        private void SpawnGeneratedAreas()
        {
            if (_generatedAreaData == null)
                return;

            for (int y = 0, i = 0; y < _areaGenerator.MapHeight; y++)
            {
                for (int x = 0; x < _areaGenerator.MapWidth; x++, i++)
                {
                    var areaData = _generatedAreaData[i];
                    InstantiateArea(areaData, x, y);
                }
            }
        }

        private void InstantiateArea(AreaData generatedData, int x, int y)
        {
            var newAreaGameObject = Instantiate(AreaPrefab, GameAreaCanvas.transform, true);
            var newArea = newAreaGameObject.GetComponent<Area>();
            newArea.SetAreaData(generatedData);
            newAreaGameObject.transform.localPosition =
                new Vector3(x * _areaSize.x + x * AreaBuffer.x, y * _areaSize.y + y * AreaBuffer.y); 
        }

        private void OnValidate()
        {
            if (AreaPrefab != null)
                _areaSize = AreaPrefab.GetComponent<RectTransform>().sizeDelta;
        }
    }
}