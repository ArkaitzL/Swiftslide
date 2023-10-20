using UnityEngine;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/Save")]
    [DisallowMultipleComponent]
    public class Save : MonoBehaviour
    {
        [SerializeField] [HideInInspector] public string nameJson = "data";
        [SerializeField] bool confirmLog = false;
        string color = "white";
        [SerializeField] SaveScript data = new SaveScript();
        static Save settings;

        public static SaveScript Data { get => settings.data; }
        public static Save Settings { get => settings; }

        // Convierte el script en Singleton
        void Instance()
        {
            if (settings != null)
            {
                Destroy(gameObject);
                return;
            }

            settings = this;
            DontDestroyOnLoad(gameObject);
        }

        // Recoge los datos y los carga desde PlayerPrefs
        private void Awake()
        {
            Instance();
            string jsonString = PlayerPrefs.GetString(nameJson);
            if (!string.IsNullOrEmpty(jsonString))
            {
                data = JsonUtility.FromJson<SaveScript>(jsonString);
                if (confirmLog)
                {
                    Debug.LogFormat("<color={0}>Datos cargados correctamente.</color>", color);
                }
            }
            else
            {
                // No se ha encontrado ningún dato en PlayerPrefs
                Debug.LogWarning("baboOn: 2.3.-No se han encontrado datos guardados.");
            }
        }

        // Al salir de la aplicación, guarda los datos en PlayerPrefs
        private void OnApplicationPause(bool pause)
        {
            if(pause) SaveData();
        }
        private void OnApplicationQuit()
        {
            SaveData();
        }
        public void SaveData() {
            string jsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(nameJson, jsonString);

            if (confirmLog)
            {
                Debug.LogFormat("<color={0}>Datos guardados en PlayerPrefs.</color>", color);
            }
        }

        // Elimina los datos de PlayerPrefs
        public void Remove()
        {
            PlayerPrefs.DeleteKey(nameJson);

            if (confirmLog)
            {
                Debug.LogFormat("<color={0}>Datos eliminados correctamente de PlayerPrefs.</color>", color);
            }
        }

        // Cambia el nombre del dato en PlayerPrefs
        public void ChangeName(string newName)
        {
            string jsonString = PlayerPrefs.GetString(nameJson);
            PlayerPrefs.DeleteKey(nameJson);
            PlayerPrefs.SetString(newName, jsonString);
            nameJson = newName;

            if (confirmLog)
            {
                Debug.LogFormat("<color={0}>Nombre del dato cambiado a \"{1}\" en PlayerPrefs.</color>", color, newName);
            }
        }
    }
}