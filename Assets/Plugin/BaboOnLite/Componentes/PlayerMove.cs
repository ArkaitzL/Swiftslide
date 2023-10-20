using UnityEngine;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/PlayerMove")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    //[HelpURL("")]

    public class PlayerMove : MonoBehaviour
    {
        [Space][Header("Movement")]

        [SerializeField] float velocity = 7;

        [Space][Header("Rotation")]

        [SerializeField] float mouseSensitivity = 2;
        [SerializeField] float maxUpAngle = 90f, maxDownAngle = -90f;

        [Space]
        [Header("Jump")]

        [SerializeField] float jump = 4;
        [SerializeField] float floorDistance = 1;
        [SerializeField] LayerMask jumpLayer = int.MaxValue;

        [Space]
        [Header("Other")]
        [SerializeField] bool freezeRotation;


        float rotationX = 0f;
        Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (freezeRotation) rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        void Update()
        {
            //Movimiento x, z--
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            if (new Vector3(x, 0, z) != Vector3.zero)
            {
                //Mover el personaje
                transform.Translate(
                    new Vector3(x, 0, z).normalized * Time.deltaTime * velocity,
                    Space.Self
                );
            }

            //Rotar--
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (mouseX != 0)
            {
                //Rotar cuerpo
                transform.Rotate(Vector3.up, mouseX);
            }

            if (mouseY != 0)
            {
                //Rotar camara
                rotationX -= mouseY;

                rotationX = Mathf.Clamp(rotationX, maxDownAngle, maxUpAngle);
                Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            }

            //Saltar con espacio--
            if (Input.GetButtonDown("Jump") && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), floorDistance+.5f, jumpLayer))
            {
                rb.AddForce(
                   new Vector3(0, jump, 0),
                   ForceMode.Impulse
                );
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.down) * floorDistance);
        }

        public void SearchFloor() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 100, jumpLayer))
            {
                floorDistance = hit.distance;
            }
        }
    }
}
