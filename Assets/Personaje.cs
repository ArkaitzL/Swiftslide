using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

public partial class Personaje
{
    [SerializeField] private float velocidad = 1;
    [SerializeField] private float rotacion = 1;
    [SerializeField] private bool inmortal = false;
    bool girando;
}
public partial class Personaje : MonoBehaviour
{
    private void Start()
    {
        velocidad = NivelesGen.data.Dificultad.velocidad;
    }
    private void Update()
    {
        if (Controles.Direccion.x != 0f || Controles.Direccion.y != 0f) {
            //Movimiento
            transform.Translate(
                Controles.Direccion * velocidad * Time.deltaTime,
                Space.World
            );
            /// *** Rotar ***
            Transform skin = transform.GetChild(0);

            float angulo = Mathf.Atan2(Controles.Direccion.y, Controles.Direccion.x) * Mathf.Rad2Deg;
            Quaternion anguloQ = Quaternion.Euler(0f, 0f, angulo - 90);

            if (skin.rotation != anguloQ && !girando) {
                girando = true;

                StartCoroutine(Tiempo(skin, anguloQ));

            }
        }

        //Deteccion de muerte
        Collider2D[] cantColliders = new Collider2D[10];
        ContactFilter2D filtro = new ContactFilter2D();
        filtro.useTriggers = true;

        int cantCollider = Physics2D.OverlapCollider(
            GetComponent<Collider2D>(),
            filtro,
            cantColliders
        );

        if (cantCollider <= 0){
            if (!inmortal)
            {
                Controlador.data.Muerte();
            }
        }
    }
    //Deteccion de dinero
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dinero")) {
            Controlador.data.Dinero(collision.gameObject);
        }
        if (collision.CompareTag("Siguiente"))
        {
            Sound.Create.Audio(4);
            collision.tag = "Untagged";
            Controlador.data.CambiarFondo();
            NivelesGen.data.Siguinte();
        }
    }
}
public partial class Personaje {
    public IEnumerator Tiempo(Transform skin, Quaternion anguloQ)
    {
        float umbral = 0.05f; // Umbral de tolerancia

        while (Quaternion.Angle(skin.rotation, anguloQ) > umbral)
        {
            skin.rotation = Quaternion.Slerp(
                skin.rotation,
                anguloQ,
                rotacion * Time.deltaTime
            );
            yield return null;
        }

        skin.rotation = anguloQ;
        girando = false;
    }
}
