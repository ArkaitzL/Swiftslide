using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/MenuUI")]
    [DisallowMultipleComponent]
    //[HelpURL("")]

    public class MenuUI : MonoBehaviour
    {
        [SerializeField] bool activeButton = true;
        private void Start()
        {
            Time.timeScale = 1;
        }

        //Teclas
        private void Update()
        {
            if (activeButton)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    PauseTime((Time.timeScale == 1) ? true : false);
                }
            }
        }

        //Cambia la escena
        public static void ChangeScene(int scene) {
            Time.timeScale = 1;
            SceneManager.LoadScene(scene);
        }
        public static void ChangeScene(string scene){
            Time.timeScale = 1;
            SceneManager.LoadScene(scene);
        }

        //Reinicia la escena
        public static void RestartScene() {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }

        //Activa y desactiva un GameObject dependiendo su anterior estado
        public static void ToggleOn(GameObject component) {
            component.SetActive(
                !component.activeSelf    
            );
        }

        //Activa o desactiva un GameObject dependiendo lo que quieras
        public static void Activate(GameObject component) {
            component.SetActive(
                true
            );
        }
        public static void Desactivate(GameObject component)
        {
            component.SetActive(
                false
            );
        }

        //Pausa o quita el pausa del juego
        public static void PauseTime(bool pause = true) {
            Time.timeScale = (pause) ? 0 : 1;
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
