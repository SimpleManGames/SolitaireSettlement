using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SolitaireSettlement
{
    public class AreaGenerator : MonoBehaviour
    {
        [System.Serializable]
        private struct AreaRegionsDefinition
        {
            [field: SerializeField, HideLabel, HorizontalGroup]
            public AreaData AreaData { get; private set; }

            [field: SerializeField, HideLabel, HorizontalGroup, Range(0, 1)]
            public float Height { get; private set; }
        }

        [field: SerializeField, Min(1), BoxGroup("Size", LabelText = "Size"), LabelText("Width")]
        public int MapWidth { get; private set; }

        [field: SerializeField, Min(1), BoxGroup("Size"), LabelText("Height")]
        public int MapHeight { get; private set; }

        [field: SerializeField, Min(0.001f), BoxGroup("Perlin", LabelText = "Perlin Noise Settings")]
        private float NoiseScale { get; set; }

        [field: SerializeField, Min(0), BoxGroup("Perlin")]
        private int Octaves { get; set; }

        [field: SerializeField, Range(0, 1), BoxGroup("Perlin")]
        private float Persistence { get; set; }

        [field: SerializeField, Min(1), BoxGroup("Perlin")]
        private float Lacunarity { get; set; }

        [field: SerializeField, BoxGroup("Perlin")]
        private Vector2 Offset { get; set; }

        [field: SerializeField, HorizontalGroup("Seed"), InlineButton("RandomSeed", "Generate New Seed")]
        private int Seed { get; set; }

        [SerializeField, HorizontalGroup("Seed"), LabelText("Randomize"), LabelWidth(70)]
        private bool newSeedOnStart = false;

        [SerializeField]
        private Gradient noiseGradient;

        [field: Title("Area Settings")]
        [field: SerializeField, InlineProperty, ListDrawerSettings(Expanded = true)]
        private List<AreaRegionsDefinition> Regions { get; set; }

        [ShowInInspector, HorizontalGroup(Title = "Debug Textures", GroupName = "NoiseTexture"),
         PreviewField(100.0f, ObjectFieldAlignment.Center), HideLabel]
        private Texture2D _noiseTexture;

        [ShowInInspector, HorizontalGroup(GroupName = "ColorTexture"),
         PreviewField(100.0f, ObjectFieldAlignment.Center), HideLabel]
        private Texture2D _colorTexture;

        [field: SerializeField, HorizontalGroup("Generate", Width = 0.1f), HideLabel]
        private bool AutoUpdate { get; set; }

        private GameObject _visualizer;
        private MeshRenderer _visualizerMeshRenderer;

        private string ButtonName => AutoUpdate ? "Auto Update Enabled" : "Generate Noise";

        [Button(Name = "@this.ButtonName"), HorizontalGroup("Generate"), DisableIf("@this.AutoUpdate")]
        private void DebugGenerateNoise()
        {
            GenerateNoise();
        }

        [Button(Name = "Generate Color"), HorizontalGroup("Generate"), DisableIf("@this.AutoUpdate")]
        private void DebugGenerateColor()
        {
            GenerateRegions(GenerateNoise());
        }

        private void RandomSeed()
        {
            EditorUtility.SetDirty(this);
            Seed = Random.Range(int.MinValue, int.MaxValue);

            if (AutoUpdate)
                GenerateRegions(GenerateNoise());
        }

        public float[,] GenerateNoise()
        {
            var noise = PerlinNoise.GeneratePerlinNoise(MapWidth, MapHeight, Seed, NoiseScale, Octaves, Persistence,
                Lacunarity, Offset);
            _noiseTexture = CreateNoiseMap(noise);

            if (_visualizer != null)
                UpdateVisualizer();

            return noise;
        }

        public AreaData[] GenerateRegions(float[,] noise)
        {
            var regionsAreaData = new AreaData[MapWidth * MapHeight];
            var colors = new Color[MapWidth * MapHeight];
            _colorTexture = new Texture2D(MapWidth, MapHeight);

            for (int y = 0, i = 0; y < MapHeight; y++)
            {
                for (var x = 0; x < MapWidth; x++, i++)
                {
                    var currentHeight = noise[x, y];
                    for (var r = 0; r < Regions.Count; r++)
                    {
                        if (currentHeight > Regions[r].Height)
                            continue;

                        regionsAreaData[i] = Regions[r].AreaData;
                        colors[i] = Regions[r].AreaData.Color;
                        break;
                    }
                }
            }

            _colorTexture.SetPixels(colors);
            _colorTexture.filterMode = FilterMode.Point;
            _colorTexture.Apply();

            return regionsAreaData;
        }

        [Button("Enable Visualizer"), ShowIf("@this._visualizer == null")]
        private void SetupVisualizer()
        {
            var gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            gameObject.name = "Visualizer";
            gameObject.transform.SetParent(transform);
            _visualizer = gameObject;
            _visualizer.transform.localPosition = new Vector3(0, 0, -0.5f);
            _visualizerMeshRenderer = _visualizer.GetComponent<MeshRenderer>();
            UpdateVisualizer();
        }

        [Button("Disable Visualizer"), ShowIf("@this._visualizer != null")]
        private void DisableVisualizer()
        {
            DestroyImmediate(_visualizer);
            _visualizer = null;
            _visualizerMeshRenderer = null;
        }

        private void UpdateVisualizer()
        {
            _visualizerMeshRenderer.sharedMaterial.mainTexture = _noiseTexture;
            _visualizer.transform.localScale = new Vector3(MapWidth, MapHeight, 1.0f);
        }

        private Texture2D CreateNoiseMap(float[,] noise)
        {
            var width = noise.GetLength(0);
            var height = noise.GetLength(1);

            var texture = new Texture2D(width, height);

            var colorMap = new Color[width * height];
            for (int y = 0, i = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++, i++)
                    colorMap[i] = noiseGradient.Evaluate(noise[x, y]);
            }

            texture.SetPixels(colorMap);
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;
        }

        private void Awake()
        {
            if (_visualizer != null)
                DisableVisualizer();

            if (newSeedOnStart)
                RandomSeed();
        }

        private void OnValidate()
        {
            if (transform.childCount > 0)
            {
                _visualizer = transform.GetChild(0).gameObject;
                _visualizerMeshRenderer = _visualizer.GetComponent<MeshRenderer>();
            }

            if (AutoUpdate)
                GenerateRegions(GenerateNoise());
        }
    }
}