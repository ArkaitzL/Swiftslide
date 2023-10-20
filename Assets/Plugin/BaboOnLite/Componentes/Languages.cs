using UnityEngine;
using TMPro;
using BaboOnLite;

namespace BaboOnLite
{
    [DefaultExecutionOrder(1)]
    [AddComponentMenu("BaboOnLite/Languages")]
    [DisallowMultipleComponent]
    //[HelpURL("")]

    public class Languages : MonoBehaviour
    {
        //Almacena los idiomas, los Textos(TMPro), el num del idioma y la instancia
        [SerializeField] Language[] languages = new Language[0];
        [SerializeField] TextMeshProUGUI[] texts = new TextMeshProUGUI[0];
        [Space]
        [Header("Selected language")]
        [SerializeField] int miLang = 0;
        [SerializeField] bool autaSave = true;

        static Languages settings;
        public static Languages Settings { get => settings; }

        private void OnValidate()
        {
            //Valida la longitud de miLang
            int length = languages.Length - 1;

            if (miLang < 0)
            {
                miLang = 0;
                return;
            }
            if (miLang >= length)
            {
                miLang = languages.Length - 1;
                return;
            }
        }
        void Instance()
        {
            if (settings == null)
            {
                settings = this;
                return;
            }

            //No se puede poner dos scripts de este tipo en la misma escena
            Debug.LogError($"baboOn: 3.1.-Existen varias instancias de languages, se ha destruido la instancia de \"{gameObject.name}\"");
            Destroy(this);
        }
        void Awake()
        {
            if (autaSave)
            {
                if (Object.FindObjectOfType<Save>() == null)
                {
                    //Tienes que tener un Save
                    Debug.LogError($"baboOn: 3.6-Para guardar el idioma es requerido el componente Save");
                    return;
                }
                miLang = Save.Data.language;
            }
            Instance();
            Validate();
            Text();
        }
        //Valida que no tenga errores
        void Validate()
        {
            int length = texts.Length;

            languages.ForEach(e => {
                if (e == null)
                {
                    //No puedes dejar en null un elemento del array Language
                    Debug.LogError($"baboOn: 3.4- No puedes dejar un campo de Idioma sin asignar");
                }
                if (e.dictionary.Length != length)
                {
                    //La longitud de los diccionarios tienen que ser iguales al de los textos
                    Debug.LogError($"baboOn: 3.2-La longitud del diccionario de {e.name}, no coincide con la cantidad de textos");
                }
            });
        }
        //Cambia el idioma
        public void Alternate()
        {
            miLang = (miLang < (languages.Length - 1))
                ? ++miLang
                : 0;
            Text();

            if (autaSave){
                if (Object.FindObjectOfType<Save>() == null)
                {
                    //Tienes que tener un Save
                    Debug.LogError($"baboOn: 3.6-Para guardar el idioma es requerido el componente Save");
                    return;
                }
                Save.Data.language = miLang;
            }

        }
        //Cambia el idioma
        public void Change(int i)
        {
            //Valida la longitud de miLang
            int length = languages.Length - 1;

            if (i >= length || i < 0)
            {
                //No hay elementos en esa posicion del array
                Debug.LogError($"baboOn: 3.5- No existe un elemento asignado, a la posicion {i} del array languages");
                return;
            }

            miLang = i;
            Text();

            if (autaSave){
                if (Object.FindObjectOfType<Save>() == null)
                {
                    //Tienes que tener un Save
                    Debug.LogError($"baboOn: 3.6-Para guardar el idioma es requerido el componente Save");
                    return;
                }
                Save.Data.language = miLang;
            }

        }
        //Escribe en los textos
        void Text()
        {
            texts.ForEach((e, i) =>
            {
                if (e == null)
                {
                    //No puedes dejar en null un elemento del array Text
                    Debug.LogError($"baboOn: 3.3- No puedes dejar un campo de Texto sin asignar");
                    return;
                }
                e.text = languages[miLang].dictionary[i];
            });
        }
    }
}
