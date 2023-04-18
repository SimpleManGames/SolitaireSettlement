using System;
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

        [field: SerializeField, AssetsOnly]
        private AreaData StartingAreaData { get; set; }

        [field: SerializeField]
        private GameObject GameAreaCanvas { get; set; }

        [field: SerializeField, AssetsOnly]
        private GameObject AreaPrefab { get; set; }

        [ShowInInspector, ReadOnly, HideIf("@this.AreaPrefab == null")]
        private Vector2 _areaSize;

        [field: SerializeField]
        private Vector2 AreaBuffer { get; set; }

        private AreaGenerator _areaGenerator;

        public int AreaCountWidth => _areaGenerator.MapWidth;
        public int AreaCountHeight => _areaGenerator.MapHeight;

        private AreaData[] _generatedAreaData;
        public List<GameObject> GeneratedAreaObjects { get; private set; } = new();
        public List<Area> GeneratedAreaComponents { get; private set; } = new();

        [field: SerializeField]
        private PolygonCollider2D CameraBoundsCollider { get; set; }

        [field: ShowInInspector]
        public Vector2 MinPosition { get; private set; }

        [field: ShowInInspector]
        public Vector2 MaxPosition { get; private set; }

        private void Start()
        {
            _areaGenerator = GetComponent<AreaGenerator>();
            _generatedAreaData = _areaGenerator.GenerateRegions(_areaGenerator.GenerateNoise());
            SpawnGeneratedAreas();

            var offset = new Vector3((AreaCountWidth * _areaSize.x + AreaCountWidth * AreaBuffer.x) / 2,
                (AreaCountHeight * _areaSize.y + AreaCountHeight * AreaBuffer.y) / 2, 0.0f);

            DetermineMaxBounds();

            foreach (var obj in GeneratedAreaObjects)
                obj.transform.position -= offset - new Vector3(_areaSize.x / 2, _areaSize.y / 2);

            var centerAreaObject = GeneratedAreaObjects.ElementAt(AreaCountWidth * AreaCountHeight / 2)
                .GetComponent<Area>();
            centerAreaObject.SetAreaData(StartingAreaData);
            centerAreaObject.Discovered = true;
            centerAreaObject.gameObject.SetActive(true);
        }


        public void DetermineMaxBounds()
        {
            var discovered = GeneratedAreaComponents.Where(c => c.Discovered).ToList();

            if (!(discovered.Count > 0))
                return;

            MinPosition = new Vector2(discovered.Min(obj => obj.transform.position.x),
                discovered.Min(obj => obj.transform.position.y));
            MaxPosition = new Vector2(discovered.Max(obj => obj.transform.position.x),
                discovered.Max(obj => obj.transform.position.y));
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
                    InstantiateArea(areaData, x, y, i);
                }
            }
        }

        private void InstantiateArea(AreaData generatedData, int x, int y, int i)
        {
            var newAreaGameObject = Instantiate(AreaPrefab, GameAreaCanvas.transform, true);
            newAreaGameObject.SetActive(false);

            var newArea = newAreaGameObject.GetComponent<Area>();
            newArea.SetAreaData(generatedData);
            newArea.Index = i;
            newAreaGameObject.transform.localPosition =
                new Vector3(x * _areaSize.x + x * AreaBuffer.x, y * _areaSize.y + y * AreaBuffer.y);
            GeneratedAreaObjects.Add(newAreaGameObject);
            GeneratedAreaComponents.Add(newArea);
        }

        private void OnValidate()
        {
            if (AreaPrefab != null)
                _areaSize = AreaPrefab.GetComponent<RectTransform>().sizeDelta;
        }
    }
}