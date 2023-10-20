using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaboOnLite
{
    [DefaultExecutionOrder(-1)]
    [AddComponentMenu("BaboOnLite/Sound")]
    [DisallowMultipleComponent]
    //[HelpURL("")]

    public class Sound : MonoBehaviour
    {
        [SerializeField] AudioClip[] sounds;
        bool vibration;

        static Sound instance;
        public static Sound Create { get => instance; set => instance = value; }
        void Instance()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                return;
            }

            //No se puede poner dos scripts de este tipo en la misma escena
            //Debug.LogError($"baboOn: 5.1.-Existen varias instancias de languages, se ha destruido la instancia de \"{gameObject.name}\"");
            Destroy(gameObject);
        }

        void Awake()
        {
            Instance();
        }

        //Crea sonidos de una sola vez. Puedes elegir que sonidos y en que posicion
        public AudioSource Audio(int sound, Vector3 position = default(Vector3), bool loop = false)
        {
            return Creator(sound, position, loop);
        }
        public AudioSource[] Audio(int[] sound, Vector3 position = default(Vector3), bool loop = false)
        {
            List<AudioSource> miSounds = new List<AudioSource>();
            sound.ForEach((i) => {
                miSounds.Add(Creator(i, position, loop));
            });
            return miSounds.ToArray();
        }

        //Crea todos los sonidos
        AudioSource Creator(int s, Vector3 p, bool loop = false) {
            if (!Save.Data.mute)
            {
                if (!sounds.Inside(s))
                {
                    //Ese sonido no esta dentro del array
                    Debug.LogError($"baboOn: 5.2.-No existe el sonido {s} dentro de Sounds");
                    return null;
                }

                GameObject soundInstance = new GameObject($"Sound-{s}");
                soundInstance.transform.SetParent(transform);

                AudioSource audioSource = soundInstance.AddComponent<AudioSource>();
                audioSource.clip = sounds[s];

                if (loop) audioSource.loop = true;
                soundInstance.transform.position = p;

                audioSource.Play();

                Destroy(soundInstance, 5f);
                return audioSource;
            }
            return null;
        }

        public void Vibration(float duration = 0)
        {
            if (vibration) {
                //Ya esta vibrando
                Debug.LogWarning($"baboOn: 5.4.-El dispositivo esta ejecutando otra vibracion");
            }

            if (SystemInfo.supportsVibration)
            {
                vibration = true;

                StartCoroutine(StartVibration());
                StartCoroutine(StopVibration(duration));

                return;
            }

            //El dispositivo no puede vibrar
            Debug.LogWarning($"baboOn: 5.3.-El dispositivo no es compatible con la vibracion");
        }
        IEnumerator StartVibration()
        {
            while (vibration)
            {
                    #if UNITY_ANDROID
                Handheld.Vibrate();
                    #endif
                yield return null;
            }
        }
        IEnumerator StopVibration(float duration)
        {
            duration.Log();
            yield return new WaitForSeconds(duration);
            vibration = false;
        }

    }

}