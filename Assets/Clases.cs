using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] //Clase para guardar informacion de una estructrura de un nivel
public class Estructura {
    public Vector2 posicion;
    public Quaternion rotacion;
    public string prefab;
    public string otro;

    public Estructura()
    {   
    }

    public Estructura(Vector2 posicion, Quaternion rotacion, string prefab, string otro)
    {
        this.posicion = posicion;
        this.rotacion = rotacion;
        this.prefab = prefab;
        this.otro = otro;
    }
}
[Serializable] //Clase para guardar informacion de un nivel
public class Nivel
{
    public int numero;
    public Estructura[] constructor;
    //***public List<(Vector2, Quaternion)> muertes;

    public Nivel(int numero, Estructura[] constructor)
    {
        this.numero = numero;
        this.constructor = constructor;
        //***this.muertes = new List<(Vector2, Quaternion)>();
    }
}
[Serializable] //Clase para guardar informacion de un la Dificultad
public class Dificultad
{
    public string nombre;
    [Range(3, 5)] public int longitudCamino;
    [Range(2, 20)] public int tamañoCamino;
    [Range(8, 20)] public int velocidad;

    public Dificultad(string nombre, int tamañoCamino, int velocidad)
    {
        this.nombre = nombre;
        this.tamañoCamino = tamañoCamino;
        this.velocidad = velocidad;
    }
}
[Serializable] 
public class Marcador {
    public Image imagen;
    public Sprite[] sprites;
}