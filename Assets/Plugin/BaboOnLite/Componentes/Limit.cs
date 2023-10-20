using UnityEngine;


namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/Limit (2D)")]
    [DisallowMultipleComponent]
    //[HelpURL("")]

    public class Limit : MonoBehaviour
    {

        [System.Serializable]
        public class Manual
        {
            [SerializeField] internal Transform left, right;
        }
        [Space]
        [SerializeField] Manual manual;
        [Space]
        [SerializeField] bool autoHeight;
        [SerializeField] bool autoInstance;

        static Limit instance;

        //Valida el uso de height junto a instance
        private void OnValidate()
        {
            if (autoInstance)
            {
                autoHeight = true;
            }
        }
        //Instancia una referencia al script
        void Instance()
        {
            if (instance == null)
            {
                instance = this;
                return;
            }

            //No se puede poner dos scripts de este tipo en la misma escena
            Debug.LogError($"baboOn: 1.1.-Existen varias instancias de languages, se ha destruido la instancia de \"{gameObject.name}\"");
            Destroy(this);
        }
        //Posiciona los elementos a los bordes de la camara
        private void Awake()
        {
            Instance();
            Validate();

            float camWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

            if (autoInstance)
            {
                if (manual.right != null || manual.left != null)
                {
                    //Se han cambiado los limites manuales por los limites automaticos
                    Debug.LogWarning("baboOn: 1.3.-Se han cambiado los limites establecidos manualmente");
                }
                manual.left = Instance("left");
                manual.right = Instance("right");
            }

            if (autoHeight)
            {
                Height(manual.left);
                Height(manual.right);
            }

            manual.left.position = new Vector3(
                Camera.main.transform.position.x - (camWidth / 2) - (manual.left.localScale.z / 2),
            0, 0);
            manual.right.position = new Vector3(
                Camera.main.transform.position.x + (camWidth / 2) + (manual.right.localScale.z / 2),
            0, 0);
        }
        //Valida que no tenga errores
        void Validate()
        {
            if (!autoInstance)
            {
                if (manual.right == null || manual.left == null)
                {
                    //No estan ni los limites automatico, ni los manuales
                    Debug.LogError("baboOn: 1.2.-No tienes asignado ningun limite");
                }
            }
        }
        //Instancia dos BoxCollider2D
        Transform Instance(string name)
        {
            GameObject ob = new GameObject(name);
            ob.AddComponent<BoxCollider>();
            ob.transform.SetParent(transform);

            return ob.transform;
        }
        //Adapta el tamaño a la altura de la camara
        void Height(Transform go)
        {
            float camHeight = Camera.main.orthographicSize * 2;

            Vector3 scale = go.localScale;
            scale.y = camHeight;
            go.localScale = scale;
        }

    }
}
