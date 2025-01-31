using Sirenix.OdinInspector.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using System.Reflection;
using System.IO;

namespace GameEvents.Editor
{
    public class GameEventEditor : OdinMenuEditorWindow
    {
        private bool _showAll = false;
        private bool _flattenFolders = true;

        private static Type[] typesToDisplay = TypeCache.GetTypesDerivedFrom(typeof(GameEventAsset<>))
            .OrderBy(m => m.Name)
            .ToArray();

        [MenuItem("Tools/Game Events/Game Event Editor", priority = - 10)]
        private static void OpenEditor() => GetWindow<GameEventEditor>();

        protected override void OnBeginDrawEditors()
        {
            MenuTree.CollapseEmptyItems();
            MenuTree.DrawSearchToolbar();
            OdinMenuTreeSelection selected = MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if(SirenixEditorGUI.ToolbarButton("New Event Type"))
                {
                    GetWindow(typeof(GameEventWindow));
                }

                bool newShowAll = SirenixEditorGUI.ToolbarToggle(_showAll, "Show All");
                if(newShowAll != _showAll)
                {
                    _showAll = newShowAll;
                    ForceMenuTreeRebuild();
                }

                bool newFlatten = SirenixEditorGUI.ToolbarToggle(_flattenFolders, "Flatten Folders");
                if(newFlatten != _flattenFolders)
                {
                    _flattenFolders = newFlatten;
                    ForceMenuTreeRebuild();
                }

                UnityEngine.Object selectedSO = null;
                if (selected != null && selected.SelectedValue is ScriptableObject so) selectedSO = so;

                if (SirenixEditorGUI.ToolbarButton("Ping") && selectedSO != null)
                {
                    EditorGUIUtility.PingObject(selectedSO);
                    Selection.activeObject = selectedSO;
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            for (int i = 0; i < typesToDisplay.Length; i++)
            {
                Type type = typesToDisplay[i];
                int count = tree.AddAllAssetsAtPath(type.Name, "Assets/", type, true, _flattenFolders).Count();
                if(count >= 1 || _showAll) tree.Add(type.Name + "/ + New", new CreateNewGameEventAsset(type));
            }
            return tree;
        }

        public class CreateNewGameEventAsset
        {
            public string Name = "New Event";
            [InlineButton(nameof(UseCurrentFolder), "Use Current")] public string Location = "Assets/Events/";

            private Type _type;
            private UnityEngine.Object _data;

            public CreateNewGameEventAsset(Type data)
            {
                _type = data;
                _data = ScriptableObject.CreateInstance(_type);
            }

            [Button("Add New Event Asset")]
            private void CreateNewData()
            {
                string lastChar = Location.Substring(Location.Length-1, 1);
                if (!lastChar.Equals("/")) Location += "/";
                CreateDirectoryIfNotFound(Location);
                AssetDatabase.CreateAsset(_data, Location + Name + ".asset");
                AssetDatabase.SaveAssets();
            }

            private void CreateDirectoryIfNotFound(string path)
            {
                bool directoryExists = Directory.Exists(path);
                if (!directoryExists) Directory.CreateDirectory(path);
            }

            private void UseCurrentFolder()
            {
                Location = GetProjectWindowPath();
            }

            private string GetProjectWindowPath()
            {
                Type projectWindowUtilType = typeof(ProjectWindowUtil);
                MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
                object obj = getActiveFolderPath.Invoke(null, new object[0]);
                string pathToCurrentFolder = obj.ToString() + "/";
                return pathToCurrentFolder;
            }
        }
    }
}