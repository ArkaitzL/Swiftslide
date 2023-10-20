using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaboOnLite;
using TMPro;
using System;

public partial class MenuJ
{
    [SerializeField] private TextMeshProUGUI fps;
    [SerializeField] private GameObject menuJ, menuP, menuS;
    [SerializeField] private Marcador sonidos, idiomas;
    [SerializeField] private GameObject[] dificultades;
    [SerializeField] private TextMeshProUGUI[] dinero;
    [SerializeField] private float tiempoMuerte = 1;

    [Header("Animaciones")]
    [SerializeField] private Animator personajeAnim;

    [Header("MenuS Animacion")]
    [SerializeField] private float duracion;
    [SerializeField] private AnimationCurve velocidad;

    [Header("Ajustes Animacion")]
    [SerializeField] private float duracionAjustes;
    [SerializeField] private AnimationCurve velocidadAjustes;
    [SerializeField] private GameObject[] ajustesComp;
    bool estadoAjustes = true;
    bool activoAjustes = true;


    public static MenuJ data;
}
public partial class MenuJ : MonoBehaviour
{
    private void Awake()
    {
        if (data != null)
        {
            Debug.LogWarning("Mas de una clase MENUJ");
            return;
        }
        data = this;
    }

    //Limitador de FPS
    private void Start() {
        //Dificultades
        dificultades[Save.Data.dificultad].SetActive(true);
 
        //Fps
        StartCoroutine(Bucle(.1f, () => {
            float fpsCont = 1f / Time.deltaTime;
            fps.text = $"{Mathf.RoundToInt(fpsCont)}";
        }));

        //Sonido botones
        StartCoroutine(Tiempo(.1f, () => {
            Button[] botones = GetComponentsInChildren<Button>(includeInactive: true);
            botones.ForEach((boton) => {
                boton.onClick.AddListener(() => Sound.Create.Audio(0));
            });
        }));


        //Otros
        CambiarMarcador(0); //Sonidos
        CambiarMarcador(1); //Idiomas
        Dinero();
    }
}
public partial class MenuJ
{
    public void Empezar() {
        menuJ.SetActive(true);
        menuP.SetActive(false);

        personajeAnim.SetBool("Correr", true);
    }
    public void CambiarDificultad(int i)
    {
        Save.Data.dificultad += i;

        if (Save.Data.dificultad > 2)
        {
            Save.Data.dificultad = 0;
        }
        if (Save.Data.dificultad < 0)
        {
            Save.Data.dificultad = 2;
        }
        MenuUI.RestartScene();
    }
    public void Dinero()
    {
        dinero.ForEach((elemento) => {
            elemento.text = Save.Data.dinero.ToString("D4");
        });
    }
    public void Morir() {
        StartCoroutine(Tiempo(tiempoMuerte, () => {
            MenuUI.RestartScene();
        }));
    }

    public void Mutear() {
        Save.Data.mute = !Save.Data.mute;
    }

    public void CambiarMarcador(int m) {
        Marcador marcador = new Marcador();
        int i = 0;

        switch (m)
        {
            case 0:
                marcador = sonidos;
                i = (Save.Data.mute) ? 1 : 0;
                break;
            case 1:
                marcador = idiomas;
                i = Save.Data.language;
                break;
        }
        marcador.imagen.sprite = marcador.sprites[i];
    }
    public void MSkin(bool estado)
    {
        StartCoroutine(Mover(
            menuS,
            (estado)
                ? new Vector2(0, 0)
                : new Vector2(0, -1800)
        ));
    }
    public void Ajustes() {
        if (activoAjustes)
        {
            estadoAjustes = !estadoAjustes;
            activoAjustes = false;

            Vector2[] posFinal = {
                new Vector2(-150, 0),
                new Vector2(0, 0),
                new Vector2(150, 0)
            };

            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(Mover(
                    ajustesComp[i],
                    (estadoAjustes)
                        ? new Vector2(325, 0)
                        : posFinal[i]
                ));
            }

            StartCoroutine(Tiempo(.5f, () => { activoAjustes = true; }));
        }
      
    }

    private IEnumerator Bucle(float tiempo, Action func) {
        yield return new WaitForSeconds(tiempo);
        func.Invoke();

        StartCoroutine(Bucle(tiempo, func));
    }
    public IEnumerator Tiempo(float tiempo, Action func)
    {
        yield return new WaitForSeconds(tiempo);
        func.Invoke();
    }
    private IEnumerator Mover(GameObject objeto, Vector2 posicionFinal) 
    {
        RectTransform rectTransform = objeto.GetComponent<RectTransform>(); 
        Vector2 posicionInicial = rectTransform.anchoredPosition;

        float tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {
            float t = tiempoPasado / duracion;
            float velocidadActual = velocidad.Evaluate(t); // Obtener la velocidad actual del gráfico
            Vector2 nuevaPosicion = Vector2.Lerp(posicionInicial, posicionFinal, velocidadActual);
            rectTransform.anchoredPosition = nuevaPosicion;
            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el objeto llegue exactamente a la posición final
        rectTransform.anchoredPosition = posicionFinal;
    }
}