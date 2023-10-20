using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BaboOnLite;

public class Prefab_Controller
{
    //Ruta de los prefabs

    //OBJETO 1
    [MenuItem("GameObject/BaboOnLite/Console (PC)")]
    private static void InstantiatePC(MenuCommand menuCommand)
    {
        Instantiate("Console/Console (PC).prefab");
    }
    //OBJETO 2
    [MenuItem("GameObject/BaboOnLite/Console (Android)")]
    private static void InstantiateAndroid(MenuCommand menuCommand)
    {
        Instantiate("Console/Console (Android).prefab");
    }

    private static void Instantiate(string name) {

        //Busca la ruta
        string path = AssetDatabase.GUIDToAssetPath(
            AssetDatabase.FindAssets("BaboOnLite")[0]
        ) + "/Prefabs/";

        //Instancia el prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path + name);
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        instance.name = prefab.name;

        //Busca el canvas y si no existe lo crea
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        }
        //Lo añade dentro del Canvas
        instance.transform.SetParent(canvas.transform, false);

        //EventSystem
        EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystem = eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<StandaloneInputModule>();
        }

        //Otros
        Undo.RegisterCreatedObjectUndo(instance, "Instantiate Custom Prefab");
        Selection.activeGameObject = instance;
    }
}
