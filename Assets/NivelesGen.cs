using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BaboOnLite;

public partial class NivelesGen //VARIABLES
{
    //[SerializeField] [Range(0, 2)] public int dificultad; //Nivel de dificultad
    [SerializeField] private Dificultad[] dificultades; //Parametros de cada niveld e dificultad
    [SerializeField] private int longitudNivel = 25; //Cantidad de partes que tiene el nivel
    [SerializeField] float longitudPlataforma = 6;
    [SerializeField] [Range(2, 25)] private int cantidadGemas = 2; //Cantidad de gemas en el nivel
    [SerializeField] [Range(0, 1)] float cambioColor = .05f;
    [SerializeField] [Range(0, 1)] float tiempoDestruccion = 1;

    private Transform siguienteTrans;
    private Dictionary<string, GameObject> prefabs;//Prefabs de las partes
    //private GameObject g, p, cgD, cgI, cpD, cpI, n, gema, invisible; //Prefabs de las partes
    private Vector2 ultimaPos = Vector2.zero; //Ultima posicion del nivel
    private Vector2 sumPosiciones = Vector2.zero; //Se usa para saber la posicion en la que tiene que aparecer la plataforma
    private int direcionGlobal = 0; //Ultima rotacion donde sea colocado una parte del nivel
    private int direcionLocal = 0; // Se usa para saber la direccion en la que apunta antes de cambiarla definitivamentre en direccionGlobal
    private int rectas = 0;//Saber cuantas rectas seguidas van

    public static NivelesGen data; //Hacer accesible este script
    public Dificultad Dificultad { get => dificultades[Save.Data.dificultad]; }//Atajo para coger la dificultad
}
public partial class NivelesGen : MonoBehaviour //Funciones de UNITY
{
    private void Awake()
    {
        //Hacer accesible este script
        if (data != null)
        {
            Debug.LogWarning("Mas de una clase NIVELES_GEN");
            return;
        }
        data = this;
        //Añadir los prefas
        prefabs = new Dictionary<string, GameObject>(){
            { "g", Resources.Load<GameObject>("Prefabs/G") },
            { "p", Resources.Load<GameObject>("Prefabs/P") },
            { "cgI", Resources.Load<GameObject>("Prefabs/CGI") },
            { "cgD", Resources.Load<GameObject>("Prefabs/CGD") },
            { "cpI", Resources.Load<GameObject>("Prefabs/CPI") },
            { "cpD", Resources.Load<GameObject>("Prefabs/CPD") },
            { "n", Resources.Load<GameObject>("Prefabs/N") },
            { "gema", Resources.Load<GameObject>("Prefabs/Gema") },
            { "invisible", Resources.Load<GameObject>("Prefabs/Invisible") },
            { "oN", Resources.Load<GameObject>("Prefabs/oN") },
            { "x", Resources.Load<GameObject>("Prefabs/X") }
        };
    }
    private void Start() {
        //Generar nivel actual
        if (Save.Data.nivelActual[Save.Data.dificultad].constructor.Length == 0) {
            Save.Data.nivelActual[Save.Data.dificultad] = Crear(Save.Data.nivelNum[Save.Data.dificultad]);
        }
        Generar(
            Save.Data.nivelActual[Save.Data.dificultad],
            false
        );

        //Generar Proximo nivel
        if (Save.Data.nivelActual != null)
            ultimaPos = Save.Data.nivelActual[Save.Data.dificultad].constructor[longitudNivel - 1].posicion;

        if (Save.Data.nivelProximo[Save.Data.dificultad].constructor.Length == 0) {
            Save.Data.nivelProximo[Save.Data.dificultad] = Crear(Save.Data.nivelNum[Save.Data.dificultad] + 1);
        }
        Generar(
            Save.Data.nivelProximo[Save.Data.dificultad],
            true,
            true
        );
    }
}
public partial class NivelesGen
{
    //Crear Proximo nivel
    public void Siguinte() { //Se llama cuando pasa de nivel
        Save.Data.nivelActual[Save.Data.dificultad] = Save.Data.nivelProximo[Save.Data.dificultad];
        Save.Data.nivelNum[Save.Data.dificultad]++;
        ultimaPos += Save.Data.nivelActual[Save.Data.dificultad].constructor[longitudNivel - 1].posicion;

        Save.Data.nivelProximo[Save.Data.dificultad] = Crear(Save.Data.nivelNum[Save.Data.dificultad] + 1);
        Generar(
            Save.Data.nivelProximo[Save.Data.dificultad]
        );

        //Destruir el anterior
        //Destroy(transform.GetChild(0).gameObject);
        StartCoroutine(Destruir(transform.GetChild(0).gameObject));
    }
    //Crear la estructura de un nivel
    private Nivel Crear(int numero) {
        List<Estructura> estructura = new List<Estructura>();
        //sumPosiciones = new Vector2(0, longitudPlataforma);
        sumPosiciones = Vector2.zero;
        direcionGlobal = 0;
        direcionLocal = 0;

        estructura.Add(
            new Estructura(
                Vector2.zero,
                Quaternion.identity,
                "invisible",
                null
        ));

        for (int i = 0; i < longitudNivel-1; i++){
            Estructura estructuraActual = new Estructura();

            //OTROS
            bool pequeño = (Random.Range(0, Dificultad.tamañoCamino) == 0)
                    ? true
                    : false;
            bool gemas = (Random.Range(0, cantidadGemas) == 0)
                    ? true
                    : false;

            //gemas
            estructuraActual.otro = (gemas)
                ? "gema"
                : null;

            //POSICION
            int direccion = Random.Range(0, Dificultad.longitudCamino);
            direccion = (direccion <= 2)
                    ? direccion
                    : 0;

            //Impedir muchas rectas
            rectas = (direccion == 0) 
                ? rectas + 1
                : 0;
            if (rectas >= 3)
            {
                direccion = Random.Range(1, 3);
                rectas = 0;
            }

            //Guardar datos
            (estructuraActual.posicion, estructuraActual.rotacion, estructuraActual.prefab) = Posicion(direccion, pequeño);

            //Correguir
            if (i != 0)
            {
                int contador = 0;
                Vector2[] proximaPosicion = {
                    new Vector2(0, longitudPlataforma),
                    new Vector2(longitudPlataforma, 0),
                    new Vector2(0, -longitudPlataforma),
                    new Vector2(-longitudPlataforma, 0)
                };

                while (estructura.Some((element) =>
                {
                    Vector2 pos1 = estructuraActual.posicion + proximaPosicion[direcionLocal];
                    Vector2 pos2 = element.posicion;
                    return (pos1 == pos2);
                }))
                {
                    if (contador >= 3)
                    {
                        Debug.LogWarning($"Se genera un nuevo nivel");
                        return Crear(numero);
                    }

                    (estructuraActual.posicion, estructuraActual.rotacion, estructuraActual.prefab) = Posicion(contador, pequeño);
                    contador++;
                };
            }

            if (longitudNivel - 2 == i) {
                estructuraActual.prefab = "oN";
                estructuraActual.otro = null;
            }

            sumPosiciones = estructuraActual.posicion;
            direcionGlobal = direcionLocal;

            estructura.Add(estructuraActual);
        }

        return new Nivel(
            numero,
            estructura.ToArray()
        );
    }
    //Generar un nivel
    private void Generar(Nivel nivel, bool guardar = true, bool rapido = false)
    {
        //Crea el inicio del nivel
        Transform inicial = Instantiate(prefabs["n"], ultimaPos, Quaternion.identity).transform;
        inicial.GetComponentInChildren<TextMeshProUGUI>().text = $"{nivel.numero}";
        inicial.parent = transform;
        if (nivel.numero != Save.Data.nivelNum[Save.Data.dificultad]) inicial.tag = "Siguiente";

        //Crea la primera plataforma
        //Instanciar(new Estructura(
        //    new Vector2(0, longitudPlataforma),
        //    Quaternion.identity,
        //    g,
        //    null
        //), -1);

        //Crea el nivel
        for (int i = 0; i < nivel.constructor.Length; i++)
        {
            Instanciar(
                nivel.constructor[i],
                i,
                inicial
            );
            /// ** if (i == longitudNivel - 2) break;
        }

        if (guardar) {
            //Cambiar a color blanco el siguiente Trans
            if (siguienteTrans != null) {
                Capa(Color.white, 0, true, rapido);
            }

            siguienteTrans = inicial;

            //Cambiar a color gris el inicial
            Capa(new Color32(128,128,128,100), -1, false, rapido);
        };

        //***nivel.muertes.ForEach(elemento => {
        //    Muerte(elemento);
        //});
    }
}
public partial class NivelesGen //FUNCIONES
{
    public void Muerte((Vector2 pos, Quaternion rot) trans) {
        Instantiate(
            prefabs["x"],
            trans.pos,
            trans.rot
        );
    }
    //Instancia las partes del nivel
    private Transform Instanciar(Estructura estructura, int nombre, Transform padre) {

        Transform instancia = Instantiate(prefabs[estructura.prefab], ultimaPos + estructura.posicion, estructura.rotacion).transform;
        instancia.name = nombre.ToString();
        instancia.parent = padre;

        if (estructura.otro != "" && estructura.otro != null)
        {
            Transform otro = Instantiate(prefabs[estructura.otro], ultimaPos + estructura.posicion, Quaternion.identity).transform;
            otro.parent = instancia;
        }

        return instancia;
    }

    //Calcular direccion
    private int Direccion(int cant) {
        cant += direcionGlobal;

        if (cant > 3) {
            return 0;
        }
        if (cant < 0) {
            return 3;
        }
        return cant;    
    }

    //Calcula la posicion donde se va a generar
    private (Vector2, Quaternion, string) Posicion(int direccion, bool pequeño) {
        Vector2 posicion = new Vector2();
        Quaternion rotacion = new Quaternion();
        string prefab = "";

        switch (direcionGlobal) // Direccion
        {
            case 0: // Adelante
                posicion = sumPosiciones + new Vector2(0, longitudPlataforma);
                rotacion = Quaternion.Euler(0, 0, 0);
                break;
            case 1: // Derecha
                posicion = sumPosiciones + new Vector2(longitudPlataforma, 0);
                rotacion = Quaternion.Euler(0, 0, -90);
                break;
            case 2: // Abajo
                posicion = sumPosiciones + new Vector2(0, -longitudPlataforma);
                rotacion = Quaternion.Euler(0, 0, -180);
                break;
            case 3: // Izquierda
                posicion = sumPosiciones + new Vector2(-longitudPlataforma, 0);
                rotacion = Quaternion.Euler(0, 0, -270);
                break;
        }

        switch (direccion) // Rotacion
        {
            case 1: // Curva  Derecha
                prefab = (pequeño)
                    ? "cpD"
                    : "cgD";
                direcionLocal = Direccion(1);
                break;
            case 2: // Curva  Izquierda
                prefab = (pequeño)
                     ? "cpI"
                     : "cgI";
                direcionLocal = Direccion(-1);
                break;
            default:
               prefab = (pequeño)
                    ? "p"
                    : "g";
                break;
        }

        return (posicion, rotacion, prefab);
    }

    IEnumerator Destruir(GameObject nivel)
    {
        List<SpriteRenderer> sr = new List<SpriteRenderer>(nivel.GetComponentsInChildren<SpriteRenderer>());

        float tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + tiempoDestruccion)
        {
            // Calcula la interpolación del color para cada SpriteRenderer hijo
            float t = (Time.time - tiempoInicio) / 1;
            foreach (SpriteRenderer spriteRenderer in sr)
            {
                Color colorObjetivo = new Color(
                    spriteRenderer.color.r,
                    spriteRenderer.color.g,
                    spriteRenderer.color.b,
                    0f
                );

                // Calcula el color interpolado para cada SpriteRenderer hijo
                Color lerpedColor = Color.Lerp(
                    spriteRenderer.color,
                    colorObjetivo,
                    t
                );

                // Actualiza el color del SpriteRenderer
                spriteRenderer.color = lerpedColor;
            }

            yield return null;
        }

        // Desactiva los SpriteRenderer después de que se hayan desvanecido completamente
        //foreach (SpriteRenderer spriteRenderer in sr)
        //{
        //    spriteRenderer.gameObject.SetActive(false);
        //}

        Destroy(nivel);
    }

    private void Capa(Color32 color, int capa, bool estado, bool rapido) {
        siguienteTrans.GetComponentsInChildren<SpriteRenderer>().ForEach(sr => {
            if (sr.tag != "Siguiente" && sr.tag != "Dinero")
            {

                if (rapido)
                {
                    sr.color = color;
                }
                else {
                    StartCoroutine(Fondo(sr, color));
                }

                //sr.color = color;
                sr.sortingOrder = capa;
            }
            if (sr.tag == "Dinero")
            {
                sr.enabled = estado;
            }
        });
        siguienteTrans.GetComponentsInChildren<BoxCollider2D>().ForEach(bc => {
            if (bc.tag != "Siguiente")
            { 
                bc.enabled = estado;
            }
        });
    }

    private IEnumerator Fondo(SpriteRenderer sr, Color color) {
        while (sr.color != color)
        {
            sr.color = Color.Lerp(
                sr.color,
                color,
                cambioColor
            );
            yield return null;
        }
    }
}