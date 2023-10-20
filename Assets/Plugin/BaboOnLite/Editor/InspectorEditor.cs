using UnityEditor;
using UnityEngine;

namespace BaboOnLite
{
    //Editor de save
    [CustomEditor(typeof(Save))]
    public class SaveEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Save ins = (Save)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Remove Data"))
            {
                ins.Remove();
            }

            if (GUILayout.Button("Change Name"))
            {
                WindowEditor.ShowWindow();
            }
        }
    }
    //Editor de language
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Language lang = (Language)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Copy"))
            {
                lang.Copy();
            }

            if (GUILayout.Button("Paste"))
            {
                lang.Paste();
            }

            if (GUILayout.Button("Paste as new"))
            {
                lang.PasteAsNew();
            }
        }
    }
    //Editor de PathMaker
    [CustomEditor(typeof(PathMaker))]
    public class PathMakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PathMaker pathMaker = (PathMaker)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Add position"))
            {
                pathMaker.Add();
            }

            if (GUILayout.Button("Save path"))
            {
                (Path path, string name) = pathMaker.Save(); 
                AssetDatabase.CreateAsset(path, $"Assets/{name}.asset");
            }
        }
    }
    //Editor de PlayerMove
    [CustomEditor(typeof(PlayerMove))]
    public class PlayerMoveEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerMove playerMove = (PlayerMove)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Search Floor"))
            {
                playerMove.SearchFloor();
            }
        }
    }
}