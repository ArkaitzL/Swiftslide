using UnityEngine;
using System.Collections.Generic;

namespace BaboOnLite
{
    public partial class SaveScript
    {
        public int dinero = 0; //Cantidad de dinero
        public List<int> skinDesbloquo = new List<int>() { 
            0
        };
        public int skinSeleccionada;
        [Range(0, 2)] public int dificultad = 0; //Dificultad del juego
        public int[] nivelNum = new int[3]; //Nivel en el que vas
        public Nivel[] nivelActual = new Nivel[3]; //Informacion del nivel actual
        public Nivel[] nivelProximo = new Nivel[3]; //Informacion del proximo nivel
    }
}
