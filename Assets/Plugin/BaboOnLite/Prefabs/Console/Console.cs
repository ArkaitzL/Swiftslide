using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BaboOnLite;

public class Console : MonoBehaviour
{
    // Se suscribe al evento de la consola
    private void OnEnable() => Application.logMessageReceived += LogMessage;
    private void OnDisable() => Application.logMessageReceived -= LogMessage;

    //Referencia los TMPro
    TextMeshProUGUI consoleText, pageText, countText, countMinText;
    GameObject max1, max2, min1;
    int l, w, e;
    bool minimize;

    void Awake()
    {
        consoleText = GameObject.Find("Message").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        pageText = GameObject.Find("Page").GetComponent<TextMeshProUGUI>();
        countText = GameObject.Find("Count").GetComponent<TextMeshProUGUI>();
        countMinText = GameObject.Find("CountMin").GetComponent<TextMeshProUGUI>();

        max1 = GameObject.Find("Message");
        max2 = GameObject.Find("Info");
        min1 = GameObject.Find("InfoMin");
        min1.SetActive(false);
    }

    private void LogMessage(string message, string stackTrace, LogType type)
    {
        //Imprime el mensaje con su color
        Dictionary<string, string> colors = new Dictionary<string, string>() {
                { "Log", "FFFFFF" },
                { "Warning", "FFA500" },
                { "Error", "FF0000" }
            };
        consoleText.text += $"<color=#{colors[type.ToString()]}>[{DateTime.Now.ToString("HH: mm: ss")}]</color>\t {message} \n";

        //Cambiar la cantidad de errores
        switch (type.ToString())
        {
            case "Log":
                l++;
                break;
            case "Warning":
                w++;
                break;
            case "Error":
                e++;
                break;
            default:
                break;
        }
        countText.text = $"<color=#FFFFFF>Log: {l}</color>     <color=#FFA500>Warning: {w}</color>     <color=#FF0000>Error: {e}</color>";
        countMinText.text = $"<color=#FFFFFF>{l}</color>\n<color=#FFA500>{w}</color>\n<color=#FF0000>{e}</color>";

        //Cambia la cantidad de paginas
        consoleText.ForceMeshUpdate();
        pageText.text = $"{consoleText.pageToDisplay}/{consoleText.textInfo.pageCount}";
    }
    public void ChangePage(int pageIndex)
    {
        //Cambia de pagina
        int page = consoleText.pageToDisplay + pageIndex;
        if (page > 0 && page <= consoleText.textInfo.pageCount)
        {
            pageText.text = $"{page}/{consoleText.textInfo.pageCount}";
            consoleText.pageToDisplay = page;
        }
    }
    public void Clear()
    {
        //Limpia la consola y las variables
        l = 0; w = 0; e = 0;
        consoleText.pageToDisplay = 1;

        consoleText.text = "";
        pageText.text = $"1/1";
        countText.text = $"<color=#FFFFFF>Log: 0</color>     <color=#FFA500>Warning: 0</color>     <color=#FF0000>Error: 0</color>";
        countMinText.text = $"<color=#FFFFFF>0</color>\n<color=#FFA500>0</color>\n<color=#FF0000>0</color>";
    }

    public void Minimize()
    {
        //Minimiza y maximiza
        minimize = !minimize;

        max1.SetActive(!minimize);
        max2.SetActive(!minimize);
        min1.SetActive(minimize);

        //Actualiza la consola
        consoleText.ForceMeshUpdate();
        pageText.text = $"{consoleText.pageToDisplay}/{consoleText.textInfo.pageCount}";
        countText.text = $"<color=#FFFFFF>Log: {l}</color>     <color=#FFA500>Warning: {w}</color>     <color=#FF0000>Error: {e}</color>";
        countMinText.text = $"<color=#FFFFFF>{l}</color>\n<color=#FFA500>{w}</color>\n<color=#FF0000>{e}</color>";
    }
}
