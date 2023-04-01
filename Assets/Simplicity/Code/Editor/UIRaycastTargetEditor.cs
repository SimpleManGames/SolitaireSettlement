using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Simplicity.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(UIRaycastTarget), false)]
    public class NonDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(base.m_Script, Array.Empty<GUILayoutOption>());
            RaycastControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}