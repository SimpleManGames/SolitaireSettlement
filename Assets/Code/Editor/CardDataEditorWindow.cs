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

        private const string STACK_ACTIONS_ASSET_PATH = SCRIPTABLE_OBJECTS_ASSET_PATH + "/Stack Actions";

        private object _previousSelectedObject;

        private OdinMenuTree _tree;

        [MenuItem("Solitaire Settlement/Data Window")]
        private static void OpenWindow()
        {
            GetWindow<CardDataEditorWindow>().Show();
        }

        private void Update()
        {
            if (_tree?.Selection.SelectedValue != null)
                return;

            if (MenuTree == null)
                return;

            // Since the CardData can rename itself through a variable field
            // We need a way to reselect if the user changes the name
            // So we store the previous selected with the _tree.Selection.SelectionChanged callback
            // And we re-find the asset as the Drawer class using the reference to the scriptable object it stores
            var childMenuRecursive = MenuTree.RootMenuItem.GetChildMenuItemsRecursive(true);
            var scriptableObjectDrawers = childMenuRecursive.Where(
                i => i.Value is ScriptableObjectAssetDrawer<CardData>);
            var matchingAsset = scriptableObjectDrawers.First(
                c =>
                {
                    var soA = ((ScriptableObjectAssetDrawer<CardData>)c.Value).ScriptableObject;
                    var soB = ((ScriptableObjectAssetDrawer<CardData>)_previousSelectedObject).ScriptableObject;
                    return soA == soB;
                });

            MenuTree.Selection.Clear();
            matchingAsset.Select();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree
            {
                Selection =
                {
                    SupportsMultiSelect = false
                },
                Config =
                {
                    DrawSearchToolbar = true,
                    AutoFocusSearchBar = false
                }
            };

            _tree.Selection.SelectionChanged += OnSelectionChanged;

            SetupCardDataTreeMenu();

            SetupStackActionsTreeMenu();

            SetupPaletteTreeMenu();

            _tree.SortMenuItemsByName(true);

            return _tree;
        }

        private void SetupCardDataTreeMenu()
        {
            var addedAssets = _tree.AddAllAssetsAtPath("Cards", CARD_DATA_ASSET_PATH,
                typeof(CardData), true, false).AddThumbnailIcons();

            var folders = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => m.ChildMenuItems.Count > 0 && addedAssets.Contains(m));

            foreach (var folder in folders)
                folder.Value = new FolderAssetDrawer<CardData>(_tree, this, folder, folder.GetFullPath());

            var nonFolderItems = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => addedAssets.Contains(m)).Except(folders);

            foreach (var cardDataItem in nonFolderItems)
            {
                var path = SCRIPTABLE_OBJECTS_ASSET_PATH + "/" + cardDataItem.GetFullPath() + ".asset";
                var cardData = AssetDatabase.LoadAssetAtPath<CardData>(path);

                cardDataItem.Value =
                    new ScriptableObjectAssetDrawer<CardData>(_tree, this, cardDataItem, cardData, null);
            }
        }

        private void SetupStackActionsTreeMenu()
        {
            var addedAssets = _tree.AddAllAssetsAtPath("Stack Actions", STACK_ACTIONS_ASSET_PATH,
                typeof(StackActionData), true, false).AddThumbnailIcons();

            var folders = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => m.ChildMenuItems.Count > 0 && addedAssets.Contains(m)).ToList();

            foreach (var folder in folders)
                folder.Value = new FolderAssetDrawer<StackActionData>(_tree, this, folder, folder.GetFullPath());

            var nonFolderItems = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => addedAssets.Contains(m)).Except(folders);

            foreach (var stackActionItem in nonFolderItems)
            {
                var path = SCRIPTABLE_OBJECTS_ASSET_PATH + "/" + stackActionItem.GetFullPath() + ".asset";
                var stackAction = AssetDatabase.LoadAssetAtPath<StackActionData>(path);

                stackActionItem.Value = new ScriptableObjectAssetDrawer<StackActionData>(_tree, this, stackActionItem,
                    stackAction, () => !stackAction.Conflict);
            }
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

        private class CreateAssetDrawer
        {
            protected OdinMenuTree tree;
            protected readonly OdinMenuEditorWindow window;
            protected OdinMenuItem item;

            public CreateAssetDrawer(OdinMenuTree tree, OdinMenuEditorWindow window, OdinMenuItem item)
            {
                this.tree = tree;
                this.window = window;
                this.item = item;
            }
        }

        private class FolderAssetDrawer<T> : CreateAssetDrawer where T : ScriptableObject
        {
            private readonly string _folderPath;

            private string AssetFolderPath => SCRIPTABLE_OBJECTS_ASSET_PATH + "/" + _folderPath;

            private static string AssetExtension => ".asset";

            private string NewAssetName => typeof(T).Name;

            private string ButtonName => $"Create New {NewAssetName} at {_folderPath}";

            [Button(ButtonSizes.Large, ButtonStyle.CompactBox, ButtonAlignment = 0.5f, Stretch = false,
                Name = "$ButtonName")]
            private void NewAsset()
            {
                CreateNewAsset();
            }

            private void CreateNewAsset()
            {
                var newInstance = CreateInstance<T>();
                AssetDatabase.CreateAsset(newInstance, AssetFolderPath + "/" + NewAssetName + AssetExtension);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                window.ForceMenuTreeRebuild();
                window.MenuTree.UpdateMenuTree();

                var childMenuRecursive = window.MenuTree.RootMenuItem.GetChildMenuItemsRecursive(true);
                var scriptableObjectDrawers = childMenuRecursive.Where(
                    i => i.Value is ScriptableObjectAssetDrawer<T>);
                var matchingAsset = scriptableObjectDrawers.First(
                    c => ((ScriptableObjectAssetDrawer<T>)c.Value).ScriptableObject == newInstance);

                // Tried to use OdinMenuEditorWindow.TrySelectMenuItemWithObject with no success even when casting to all sorts of things that may have worked
                // So instead just brute force it by running the code that would be ran if TrySelectMenuItemWithObject was successful
                window.MenuTree.Selection.Clear();
                matchingAsset.Select();
            }

            public FolderAssetDrawer(OdinMenuTree tree, OdinMenuEditorWindow window,
                OdinMenuItem item, string folderPath) : base(tree, window, item)
            {
                _folderPath = folderPath;
            }
        }

        [Serializable]
        private class ScriptableObjectAssetDrawer<T> : CreateAssetDrawer where T : ScriptableObject
        {
            [field: SerializeField, InlineEditor(InlineEditorModes.FullEditor, Expanded = true,
                        ObjectFieldMode = InlineEditorObjectFieldModes.CompletelyHidden)]
            public T ScriptableObject { get; private set; }

            private readonly Func<bool> _validEntryCheck;

            protected bool ValidEntry => _validEntryCheck?.Invoke() ?? true;

            public ScriptableObjectAssetDrawer(OdinMenuTree tree, OdinMenuEditorWindow window,
                OdinMenuItem item, T scriptableObject, Func<bool> validEntryCheck)
                : base(tree, window, item)
            {
                ScriptableObject = scriptableObject;
                _validEntryCheck += validEntryCheck;
                item.IconGetter += IconGetter;
            }

            private Texture IconGetter()
            {
                return ValidEntry
                    ? EditorGUIUtility.IconContent("d_ScriptableObject Icon").image
                    : EditorGUIUtility.IconContent("console.erroricon").image;
            }

            public override bool Equals(object obj)
            {
                if (obj is ScriptableObjectAssetDrawer<T> cast)
                    return ScriptableObject.Equals(cast.ScriptableObject);

                return false;
            }

            protected bool Equals(ScriptableObjectAssetDrawer<T> other)
            {
                return Equals(_validEntryCheck, other._validEntryCheck) &&
                       EqualityComparer<T>.Default.Equals(ScriptableObject, other.ScriptableObject);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(_validEntryCheck, ScriptableObject);
            }
        }
    }
}