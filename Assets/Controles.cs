using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public partial class Controles
{
    [SerializeField] private float deslizamientoMin = 50f;
    [HideInInspector] public Vector2 direccion = Vector2.up;

    private Dictionary<string, bool> activo = new Dictionary<string, bool>() {
        { "mover", true },
        { "menuS", true }
    };
    private int menu = 1;
    private Vector2 inicialPos;
    private Vector2[] direcciones = { // Arriba, Abajo, Derecha, Izquierda
        Vector2.up,
        Vector2.down,
        Vector2.right,
        Vector2.left
    };

    public static Controles data;
    public static Vector2 Direccion { get => data.direccion;}
}
public partial class Controles : MonoBehaviour
{
    private void Awake()
    {
        if (data != null) {
            Debug.LogWarning("Mas de una clase CONTROLES");
            return;
        }
        data = this;
    }

    private void Update()
    {
        PC();
        Movil();
    }
}

public partial class Controles
{
    private void Movil() {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //INICIO
            if (touch.phase == TouchPhase.Began)
            {
                inicialPos = touch.position;
            }
            //FIN
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 finalPos = touch.position;
                Vector2 apuntado = finalPos - inicialPos;

                if (apuntado.magnitude < deslizamientoMin) return;
                apuntado.Normalize();

                if (apuntado.y > 0.5f) {
                    Dirigir(0);
                }
                else if (apuntado.y < -0.5f){
                    Dirigir(1);
                }
                else if (apuntado.x > 0.5f) {
                    Dirigir(2);
                }
                else if (apuntado.x < -0.5f){
                    Dirigir(3);
                }
            }
        }
    }
    private void PC(){
        bool[] teclas = {
            Input.GetKeyDown(KeyCode.W),
            Input.GetKeyDown(KeyCode.S),
            Input.GetKeyDown(KeyCode.D),
            Input.GetKeyDown(KeyCode.A)
        };
        teclas.ForEach((tecla, index) => {
            if (tecla){
                Dirigir(index);
            }
        });
    }

    private void Dirigir(int index) {
        switch (menu)
        {
            case 0:
                //Juego
                if (activo["mover"]) {
                    Sound.Create.Audio(3);
                    direccion = direcciones[index];
                    StartCoroutine(Cuenta("mover", 0.3f));
                }
             
                break;
            case 1:
                //Menu
                switch (index)
                {
                    case 0:
                        Sound.Create.Audio(3);
                        menu = 0;
                        direccion = direcciones[0];
                        MenuJ.data.Empezar();
                        Controlador.data.CambiarFondo();
                        break;
                    case 1:
                        if (activo["menuS"]){
                            Sound.Create.Audio(0);
                            menu = 2;
                            MenuJ.data.MSkin(true);
                            StartCoroutine(Cuenta("menuS", 0.5f));
                        }
                        break;
                    case 2:
                        Sound.Create.Audio(0);
                        MenuJ.data.CambiarDificultad(+1);
                        break;
                    case 3:
                        Sound.Create.Audio(0);
                        MenuJ.data.CambiarDificultad(-1);
                        break;
                }
                break;
            case 2:
                //Skins
                switch (index)
                {
                    case 0:
                        if (activo["menuS"]) {
                            Sound.Create.Audio(0);
                            menu = 1;
                            MenuJ.data.MSkin(false);
                            StartCoroutine(Cuenta("menuS", 0.5f));
                        }
                        break;
                    case 1:
                        //Cambiar entre menus
                        break;
                    case 2:
                        //Cambiar entre menus
                        break;
                    case 3:

                        break;
                }
                break;
        }
    }

    private IEnumerator Cuenta(string nombre, float cuenta) {
        activo[nombre] = false;
        yield return new WaitForSeconds(cuenta);
        activo[nombre] = true;
    }
}