using System;
using System.Collections.Generic;
using System.Linq;
using Simplicity.GameEvent;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardDataEditorWindow : OdinMenuEditorWindow
    {
        private const string SCRIPTABLE_OBJECTS_ASSET_PATH = "Assets/Data/Scriptable Objects";
        private const string AREA_DATA_ASSET_PATH = SCRIPTABLE_OBJECTS_ASSET_PATH + "/Areas";
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

            if (_previousSelectedObject is CreateAssetDrawer drawer)
                drawer.Reselect();
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

            SetupAreaDataTreeMenu();

            SetupCardDataTreeMenu();

            SetupStackActionsTreeMenu();

            SetupPaletteTreeMenu();

            _tree.SortMenuItemsByName(true);

            return _tree;
        }

        private void SetupAreaDataTreeMenu()
        {
            SetupTreeMenu<AreaData>("Areas", AREA_DATA_ASSET_PATH, null);
        }

        private void SetupCardDataTreeMenu()
        {
            SetupTreeMenu<CardData>("Cards", CARD_DATA_ASSET_PATH, null);
        }

        private void SetupStackActionsTreeMenu()
        {
            SetupTreeMenu<StackActionData>("Stack Actions", STACK_ACTIONS_ASSET_PATH, asset => !asset.Conflict);
        }

        private void SetupPaletteTreeMenu()
        {
            SetupTreeMenu<CardPaletteData>("Palette", PALETTE_ASSET_PATH, null);
        }

        private void SetupTreeMenu<T>(string menuPath, string assetPath, Func<T, bool> validEntryCheck)
            where T : ScriptableObject
        {
            var addedAssets = _tree.AddAllAssetsAtPath(menuPath, assetPath,
                typeof(T), true, false).AddThumbnailIcons();

            var folders = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => m.ChildMenuItems.Count > 0 && addedAssets.Contains(m)).ToList();

            foreach (var folder in folders)
                folder.Value = new FolderAssetDrawer<T>(_tree, this, folder, folder.GetFullPath());

            var nonFolderItems = _tree.RootMenuItem.GetChildMenuItemsRecursive(true)
                .Where(m => addedAssets.Contains(m)).Except(folders);

            foreach (var item in nonFolderItems)
            {
                var path = SCRIPTABLE_OBJECTS_ASSET_PATH + "/" + item.GetFullPath() + ".asset";
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);

                item.Value =
                    new ScriptableObjectAssetDrawer<T>(_tree, this, item, asset, validEntryCheck);
            }
        }

        private void OnSelectionChanged(SelectionChangedType type)
        {
            if (type == SelectionChangedType.ItemAdded)
                _previousSelectedObject = _tree.Selection.SelectedValue;
        }

        private abstract class CreateAssetDrawer
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

            public abstract void Reselect();
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

                item.IconGetter += IconGetter;
            }

            private Texture IconGetter()
            {
                return item.Toggled
                    ? EditorGUIUtility.IconContent("FolderOpened On Icon").image
                    : EditorGUIUtility.IconContent("Folder On Icon").image;
            }

            public override void Reselect()
            {
            }
        }

        [Serializable]
        private class ScriptableObjectAssetDrawer<T> : CreateAssetDrawer where T : ScriptableObject
        {
            [field: SerializeField, InlineEditor(InlineEditorModes.FullEditor, Expanded = true,
                        ObjectFieldMode = InlineEditorObjectFieldModes.CompletelyHidden)]
            public T ScriptableObject { get; private set; }

            private readonly Func<T, bool> _validEntryCheck;

            protected bool ValidEntry => _validEntryCheck?.Invoke(ScriptableObject) ?? true;

            public ScriptableObjectAssetDrawer(OdinMenuTree tree, OdinMenuEditorWindow window,
                OdinMenuItem item, T scriptableObject, Func<T, bool> validEntryCheck)
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

            public override void Reselect()
            {
                // Since the ScriptableObjects can rename itself through a variable field
                // We need a way to reselect if the user changes the name
                // So we store the previous selected with the _tree.Selection.SelectionChanged callback
                // And we re-find the asset as the Drawer class using the reference to the scriptable object it stores
                var childMenuRecursive = window.MenuTree.RootMenuItem.GetChildMenuItemsRecursive(true);
                var scriptableObjectDrawers = childMenuRecursive.Where(
                    i => i.Value is ScriptableObjectAssetDrawer<T>).ToList();
                if (scriptableObjectDrawers.Count == 0)
                    return;

                var matchingAsset = scriptableObjectDrawers.First(
                    c =>
                    {
                        var soA = ((ScriptableObjectAssetDrawer<T>)c.Value).ScriptableObject;
                        var soB = ScriptableObject;
                        return soA == soB;
                    });

                window.MenuTree.Selection.Clear();
                matchingAsset.Select();
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