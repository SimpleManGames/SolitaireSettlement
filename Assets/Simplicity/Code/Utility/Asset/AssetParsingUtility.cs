using System.Collections.Generic;
using UnityEditor;

namespace Simplicity.Utility
{
    public static class AssetParsingUtility
    {
        public static T1 FindAssetsByType<T0, T1>() where T0 : UnityEngine.Object where T1 : ICollection<T0>, new()
        {
            T1 assets = new();
            var guids = AssetDatabase.FindAssets($"t:{typeof(T0)}");

            for (var i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<T0>(assetPath);
                if (asset != null)
                    assets.Add(asset);
            }

            return assets;
        }
    }
}