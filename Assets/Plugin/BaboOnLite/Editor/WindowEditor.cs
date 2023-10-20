using UnityEditor;
using UnityEngine;

namespace BaboOnLite 
{
    //Ventana de Save
    public class WindowEditor : EditorWindow
    {
        string newName;

        internal static void ShowWindow()
        {
            GetWindow<WindowEditor>("Change Name");
        }

        private void OnGUI()
        {
            Save save = FindObjectOfType<Save>();

            EditorGUILayout.LabelField($"Cambiar \"{save.nameJson}\" como nombre del archivo:");
            newName = EditorGUILayout.TextField("Enter new name:", newName);

            if (GUILayout.Button("Change"))
            {
                if (save != null)
                {
                    save.ChangeName(newName);
                }
                Close();
            }
        }
    }
}