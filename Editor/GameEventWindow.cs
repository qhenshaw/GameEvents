using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameEvents.Editor
{
    public class GameEventWindow : EditorWindow
    {
        private string _type;
        private string _typeCapitalized;
        private string _path = "Assets/Scripts/CustomEventTypes/";

        [MenuItem("Tools/Game Events/Create New Event Scripts")]
        public static void ShowWindow()
        {
            GetWindow(typeof(GameEventWindow));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Enter a valid C# type with no spaces");
            string input = EditorGUILayout.TextField("Type", _type);
            if (input != null)
            {
                _type = input.Trim();
                _typeCapitalized = input[0].ToString().ToUpper() + input.Substring(1);
            }
            EditorGUILayout.LabelField("Enter path to event scripts or use default");
            string path = EditorGUILayout.TextField("Path", _path);
            if (path != null)
            {
                _path = path.Trim();
                string lastChar = _path.Substring(_path.Length - 1, 1);
                if (!lastChar.Equals("/")) _path += "/";
            }

            if (GUILayout.Button("Generate"))
            {
                CreateDirectoryIfNotFound(_path);
                CreateEvent();
                CreateCaller();
                CreateListener();
                AssetDatabase.Refresh();
            }
        }

        private void CreateEvent()
        {
            string path = $"{_path}{_typeCapitalized}EventAsset.cs";
            Debug.Log($"Creating Event: {path}");
            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace GameEvents");
                outfile.WriteLine("{");
                outfile.WriteLine($"    [CreateAssetMenu(menuName = \"Events/{_typeCapitalized} Event Asset\")]");
                outfile.WriteLine($"    public class {_typeCapitalized}EventAsset : GameEventAsset<{_type}> {{}}");
                outfile.WriteLine("}");
            }
        }

        private void CreateCaller()
        {
            string path = $"{_path}{_typeCapitalized}EventCaller.cs";
            Debug.Log($"Creating Caller: {path}");
            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace GameEvents");
                outfile.WriteLine("{");
                outfile.WriteLine($"    public class {_typeCapitalized}EventCaller : GameEventCaller<{_type}> {{}}");
                outfile.WriteLine("}");
            }
        }

        private void CreateListener()
        {
            string path = $"{_path}{_typeCapitalized}EventListener.cs";
            Debug.Log($"Creating Listener: {path}");
            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace GameEvents");
                outfile.WriteLine("{");
                outfile.WriteLine($"    public class {_typeCapitalized}EventListener : GameEventListener<{_type}> {{}}");
                outfile.WriteLine("}");
            }
        }

        private void CreateDirectoryIfNotFound(string path)
        {
            bool directoryExists = Directory.Exists(path);
            if (!directoryExists) Directory.CreateDirectory(path);
        }
    }
}