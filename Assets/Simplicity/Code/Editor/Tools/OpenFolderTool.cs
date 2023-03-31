using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BountyHunters
{
    public class OpenFolderTool
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId)
        {
            var e = Event.current;
            if (e?.shift != true)
                return false;

            var obj = EditorUtility.InstanceIDToObject(instanceId);
            var path = AssetDatabase.GetAssetPath(obj);
            if (AssetDatabase.IsValidFolder(path))
                EditorUtility.RevealInFinder(path);

            return true;
        }
    }
}