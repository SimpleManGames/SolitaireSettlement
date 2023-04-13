using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardDataEditorWindow : OdinMenuEditorWindow
    {
        private const string SCRIPTABLE_OBJECTS_ASSET_PATH = "Assets/Data/Scriptable Objects";
        private const string CARD_DATA_ASSET_PATH = SCRIPTABLE_OBJECTS_ASSET_PATH + "/Cards";
        private const string PALETTE_ASSET_PATH = CARD_DATA_ASSET_PATH + "/Palette";

        private object _previousSelectedObject;

        private OdinMenuTree _tree;

        [MenuItem("Solitaire Settlement/Card Data Window")]
        private static void OpenWindow()
        {
            GetWindow<CardDataEditorWindow>().Show();
        }

        private void Update()
        {
            if (_tree.Selection.SelectedValue == null)
                TrySelectMenuItemWithObject(_previousSelectedObject);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree
            {
                Selection =
                {
                    SupportsMultiSelect = false
                }
            };

            _tree.AddAllAssetsAtPath("Cards", CARD_DATA_ASSET_PATH,
                typeof(CardData), true, false).AddThumbnailIcons();

            var folders = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => m.ChildMenuItems.Count > 0);

            foreach (var folder in folders)
                folder.Value = new CardDataFolderUtility(folder.GetFullPath(), _tree, this);

            _tree.AddAllAssetsAtPath("Palette", PALETTE_ASSET_PATH,
                typeof(CardPaletteData), true, false);

            _tree.Selection.SelectionChanged += OnSelectionChanged;

            return _tree;
        }

        private void OnSelectionChanged(SelectionChangedType type)
        {
            if (type == SelectionChangedType.ItemAdded)
                _previousSelectedObject = _tree.Selection.SelectedValue;
        }

        private class CardDataFolderUtility
        {
            private readonly string _folderPath;

            private OdinMenuTree _tree;
            private readonly OdinMenuEditorWindow _window;

            private string AssetFolderPath => SCRIPTABLE_OBJECTS_ASSET_PATH + "/" + _folderPath;

            private string NewCardDataName => "New Card Data.asset";

            [Button(ButtonSizes.Large)]
            public void NewCardData()
            {
                var newInstance = CreateInstance<CardData>();
                AssetDatabase.CreateAsset(newInstance, AssetFolderPath + "/" + NewCardDataName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                _window.TrySelectMenuItemWithObject(newInstance);
            }

            public CardDataFolderUtility(string folderPath, OdinMenuTree tree, OdinMenuEditorWindow window)
            {
                _folderPath = folderPath;
                _tree = tree;
                _window = window;
            }
        }
    }
}