using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Simplicity.GameEvent;
using Simplicity.Singleton;
using Simplicity.UI;
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
        private List<GameObject> _generatedAreaObjects = new List<GameObject>();

        [field: SerializeField]
        private PolygonCollider2D CameraBoundsCollider { get; set; }

        private Vector2 _minPosition;

        private Vector2 _maxPosition;

        private void Start()
        {
            _areaGenerator = GetComponent<AreaGenerator>();
            _generatedAreaData = _areaGenerator.GenerateRegions(_areaGenerator.GenerateNoise());
            SpawnGeneratedAreas();

            _minPosition.x = _generatedAreaObjects.Min(obj => obj.transform.position.x);
            _minPosition.y = _generatedAreaObjects.Min(obj => obj.transform.position.y);
            _maxPosition.x = _generatedAreaObjects.Max(obj => obj.transform.position.x);
            _maxPosition.y = _generatedAreaObjects.Max(obj => obj.transform.position.y);

            CameraBoundsCollider.points = new[]
            {
                new Vector2(_minPosition.x, _minPosition.y),
                new Vector2(_minPosition.x, _maxPosition.y),
                new Vector2(_maxPosition.x, _maxPosition.y),
                new Vector2(_maxPosition.x, _minPosition.y),
            };
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
            _generatedAreaObjects.Add(newAreaGameObject);
        }

        private void OnValidate()
        {
            if (AreaPrefab != null)
                _areaSize = AreaPrefab.GetComponent<RectTransform>().sizeDelta;
        }
    }
}