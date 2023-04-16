using UnityEngine;

namespace SolitaireSettlement
{
    public static class PerlinNoise
    {
        public static float[,] GeneratePerlinNoise(int width, int height, int seed, float scale, int octaves,
            float persistence, float lacunarity, Vector2 offset)
        {
            var prng = new System.Random(seed);
            var octaveOffsets = new Vector2[octaves];

            for (var i = 0; i < octaves; i++)
            {
                var offsetX = prng.Next(-10000, 10000) + offset.x;
                var offsetY = prng.Next(-10000, 10000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            var noise = new float[width, height];

            if (scale <= 0)
                scale = 0.001f;

            var minNoise = float.MaxValue;
            var maxNoise = float.MinValue;

            var halfWidth = width / 2.0f;
            var halfHeight = height / 2.0f;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var amplitude = 1.0f;
                    var frequency = 1.0f;
                    float noiseHeight = 0;

                    for (var o = 0; o < octaves; o++)
                    {
                        var sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[o].x;
                        var sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[o].y;

                        var perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                        noiseHeight += perlin * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoise)
                        maxNoise = noiseHeight;
                    else if (noiseHeight < minNoise)
                        minNoise = noiseHeight;

                    noise[x, y] = noiseHeight;
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    noise[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noise[x, y]);
                }
            }

            return noise;
        }
    }
}