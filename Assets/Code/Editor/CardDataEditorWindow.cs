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

        private const string STACK_ACTIONS_ASSET_PATH = SCRIPTABLE_OBJECTS_ASSET_PATH + "/Stack Actions";

        private object _previousSelectedObject;

        private OdinMenuTree _tree;

        [MenuItem("Solitaire Settlement/Card Data Window")]
        private static void OpenWindow()
        {
            GetWindow<CardDataEditorWindow>().Show();
        }

        private void Update()
        {
            if (_tree?.Selection.SelectedValue == null)
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

            _tree.Selection.SelectionChanged += OnSelectionChanged;

            SetupCardDataTreeMenu();

            SetupStackActionsTreeMenu();

            SetupPaletteTreeMenu();

            return _tree;
        }

        private void SetupCardDataTreeMenu()
        {
            var addedAssets = _tree.AddAllAssetsAtPath("Cards", CARD_DATA_ASSET_PATH,
                typeof(CardData), true, false).AddThumbnailIcons();

            var folders = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => m.ChildMenuItems.Count > 0 && addedAssets.Contains(m));

            foreach (var folder in folders)
                folder.Value = new CreateAssetDrawer<CardData>(folder.GetFullPath(), _tree, this);
        }

        private void SetupStackActionsTreeMenu()
        {
            var addedAssets = _tree.AddAllAssetsAtPath("Stack Actions", STACK_ACTIONS_ASSET_PATH,
                typeof(StackActionData), true, false).AddThumbnailIcons();

            var folders = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => m.ChildMenuItems.Count > 0 && addedAssets.Contains(m));

            foreach (var folder in folders)
                folder.Value = new CreateAssetDrawer<StackActionData>(folder.GetFullPath(), _tree, this);
        }

        private void SetupPaletteTreeMenu()
        {
            _tree.AddAllAssetsAtPath("Palette", PALETTE_ASSET_PATH,
                typeof(CardPaletteData), true, false);
        }

        private void OnSelectionChanged(SelectionChangedType type)
        {
            if (type == SelectionChangedType.ItemAdded)
                _previousSelectedObject = _tree.Selection.SelectedValue;
        }

        private class CreateAssetDrawer<T> where T : ScriptableObject
        {
            private readonly string _folderPath;

            private OdinMenuTree _tree;
            private readonly OdinMenuEditorWindow _window;

            private string AssetFolderPath => SCRIPTABLE_OBJECTS_ASSET_PATH + "/" + _folderPath;

            private static string AssetExtension => ".asset";

            private string NewAssetName => typeof(T).Name;

            private string ButtonName => $"Create New {NewAssetName} at {_folderPath}";

            [Button(ButtonSizes.Large, ButtonStyle.CompactBox, ButtonAlignment = 0.5f, Stretch = false,
                Name = "$ButtonName")]
            public void New()
            {
                CreateNewAsset();
            }

            private void CreateNewAsset()
            {
                var newInstance = CreateInstance<T>();
                AssetDatabase.CreateAsset(newInstance, AssetFolderPath + "/" + NewAssetName + AssetExtension);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                _window.TrySelectMenuItemWithObject(newInstance);
            }

            public CreateAssetDrawer(string folderPath, OdinMenuTree tree, OdinMenuEditorWindow window)
            {
                _folderPath = folderPath;
                _tree = tree;
                _window = window;
            }
        }
    }
}