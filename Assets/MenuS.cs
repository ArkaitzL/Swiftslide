using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using UnityEngine.UI;
using TMPro;

public partial class MenuS
{
    [SerializeField] Transform menuS;

    //Skins menu
    [SerializeField] Sprite[] misSkins = new Sprite[0];
    [SerializeField] int precio = 100; 
    [SerializeField] GameObject menuPref, skinPref;

    //Comprar
    [SerializeField] GameObject comprarBtn;

    //Personaje
    SpriteRenderer personaje;
}
public partial class MenuS : MonoBehaviour
{
    private void Start()
    {
        if (Save.Data.skinSeleccionada >= misSkins.Length) Save.Data.skinSeleccionada = 0;

        personaje = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<SpriteRenderer>();

        Transform menu = Instantiate(menuPref, menuS).transform;
        menu.name = $"Menu * {1}";

        comprarBtn.GetComponent<Button>().onClick.AddListener(() => Comprar(precio));
        comprarBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{precio}";

        misSkins.ForEach((skinE, i) => {
            Transform skin = Instantiate(skinPref, menu).transform;
            skin.name = $"Skin * {i}";

            skin.GetChild(0).GetComponent<Image>().sprite = skinE;
            skin.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Seleccionar(i));

            if (Save.Data.skinDesbloquo.Some((e) =>
                i == e
            ))
            {
                menuS.GetChild(1).GetChild(i).GetChild(2).gameObject.SetActive(false);
            }
        });

        personaje.sprite = misSkins[Save.Data.skinSeleccionada];
        menuS.GetChild(1).GetChild(Save.Data.skinSeleccionada).GetChild(3).gameObject.SetActive(true);
    }
}
public partial class MenuS
{
    public void Seleccionar(int i) {
        if (i == Save.Data.skinSeleccionada) return;

        personaje.sprite = misSkins[i];
        menuS.GetChild(1).GetChild(Save.Data.skinSeleccionada).GetChild(3).gameObject.SetActive(false);
        menuS.GetChild(1).GetChild(i).GetChild(3).gameObject.SetActive(true);

        Save.Data.skinSeleccionada = i;
    }

    public void Comprar(int cant) {
        //Marcar Cual esta seleccionado
        if (misSkins.Length <= Save.Data.skinDesbloquo.Count){
            Debug.Log("Todos desbloqueados");
            return;
        }
        if (Save.Data.dinero >= cant) {
            int num = 0;
            do {
                num = Random.Range(0, misSkins.Length);
            } while (Save.Data.skinDesbloquo.Some((i) => 
                i == num
            ));

            Save.Data.dinero -= cant;
            menuS.GetChild(1).GetChild(num).GetChild(2).gameObject.SetActive(false);
            MenuJ.data.Dinero();
            Save.Data.skinDesbloquo.Add(num);
        }
    }
}