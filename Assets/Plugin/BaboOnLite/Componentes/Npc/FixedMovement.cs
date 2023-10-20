using System.Collections;
using UnityEngine;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/ Npc/FixedMovement (NPC)")]
    [DisallowMultipleComponent]
    //[HelpURL("")]
    public class FixedMovement : MonoBehaviour
    {
        [SerializeField] Path path;
        [Space]
        [SerializeField] float waitTime = 2;
        [SerializeField] float speed = 2, rotateSpeed = 2;
        int i;
        [Space]
        [SerializeField] bool move2D;
        [SerializeField] bool goBack;
        bool back;

        private void Start()
        {
            //Validar que tenga una ruta
            if (path == null)
            {
                Debug.LogError($"baboOn: 4.1.-No hay una ruta establecida");
                return;
            }

            //Se pone en la posicion inicial
            transform.position = path.positions[i++];
            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            yield return new WaitForSeconds(waitTime);

            //Calcula la direccion a la que se va a mover
            Vector3 targetPos = (back) 
                ? path.positions[--i]
                : path.positions[i++];
            Vector3 direction = (targetPos - transform.position).normalized;


            if (move2D)
            {
                //Rota a la direccion a donde se va a mover 2D
                float angle;
                do
                {
                    angle = Mathf.Atan2(
                        direction.y,
                        direction.x
                    ) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.Euler(0f, 0f, angle - 90f),
                        rotateSpeed * Time.deltaTime
                    );

                    yield return null;
                } while (Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f)) > 0.02f);
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            }
            else
            {
                //Rota a la direccion a donde se va a mover 3D
                while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) > 0.02f)
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(direction),
                        rotateSpeed * Time.deltaTime
                    );
                    yield return null;
                }
                transform.rotation = Quaternion.LookRotation(direction);

            }

            //Se mueva a su proxima posicion
            while (Vector3.Distance(targetPos, transform.position) > 0.1f)
            {
                transform.Translate(direction * speed * Time.deltaTime, Space.World);
                yield return null;
            }
            transform.position = targetPos;

            //Reinicia el proceso
            if (goBack) {
                if (i == path.positions.Length) {
                    back = true;
                    i--;
                }
                if (i == 0) { 
                    back = false;
                    i = 1;
                }
            }
            else {
                if (i == path.positions.Length) i = 0;
            }
            StartCoroutine(Move());
        }

        [ContextMenu("Cancel Movement")]
        public void CancelMovement()
        {
            if (back) i++;
            StopAllCoroutines();
        }

        [ContextMenu("Start Movement")]
        public void StartMovement() {
            StartCoroutine(Move());
        }
    }
}