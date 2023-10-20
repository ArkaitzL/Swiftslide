using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public partial class Controlador
{
    [SerializeField][Range(0,1)] float cambioColor = .05f;
    [SerializeField] Color32[] coloresFondo = new Color32[]{
        new Color32(108, 195, 101, 255), // Verde suave
        new Color32(239, 157, 84, 255), // Naranja suave
        new Color32(220, 98, 98, 255), // Rojo suave
        new Color32(183, 186, 64, 255), // Amarillo suave
        new Color32(100, 169, 199, 255), // Cian suave
        new Color32(199, 100, 169, 255), // Magenta suave
        new Color32(222, 130, 186, 255), // Rosa suave
        new Color32(234, 202, 96, 255), // Amarillo suave
};

    GameObject personaje;
    private bool muerto;

    //Hacer accesible este script
    public static Controlador data;
}
public partial class Controlador : MonoBehaviour
{
    private void Awake()
    {
        //Hacer accesible este script
        if (data != null)
        {
            Debug.LogWarning("Mas de una clase CONTROLES");
            return;
        }
        data = this;
    }
    private void Start()
    {
        //BuscarPlayer
        personaje = GameObject.FindGameObjectWithTag("Player");
        //Limitar FPS
        Application.targetFrameRate = 120;
        //Cambiar Fondo
        Camera.main.backgroundColor = coloresFondo[Save.Data.dificultad];
        //Controlador.data.CambiarFondo(Save.Data.dificultad);
    }
}
public partial class Controlador
{
    public void Muerte()
    {
        if (!muerto) {
            Quaternion rotacion = Quaternion.Euler(0, 0, Random.Range(0, 360));
            StartCoroutine(MenuJ.data.Tiempo(.1f, () => {
                //***Save.Data.nivelActual[Save.Data.dificultad].muertes.Add(
                //    (personaje.transform.position, rotacion)
                //);
                Destroy(personaje, .1f);
                NivelesGen.data.Muerte((personaje.transform.position, rotacion));
            }));
            personaje.GetComponent<Personaje>().enabled = false;
            personaje.GetComponent<Controles>().enabled = false;
            MenuJ.data.Morir();
            Sound.Create.Audio(1);
            muerto = true;
        }
    }

    public void Dinero(GameObject objeto, int cantidad = 1) {
        Sound.Create.Audio(2);
        Save.Data.dinero += cantidad;
        Destroy(objeto);
        MenuJ.data.Dinero();
    }
    [ContextMenu("CambioFondo")]
    public void CambiarFondo() {
        StartCoroutine(
            Fondo(
                coloresFondo[Random.Range(0, coloresFondo.Length)]
            )
        );
    }
    public void CambiarFondo(int color) {
        StartCoroutine(
            Fondo(
                coloresFondo[color]
            )
        );
    }
    private IEnumerator Fondo(Color32 color)
    {
        while (Camera.main.backgroundColor != color)
        {
            Camera.main.backgroundColor = Color.Lerp(
                Camera.main.backgroundColor,
                color,
                cambioColor
            );
            yield return null;
        }
    }
}
