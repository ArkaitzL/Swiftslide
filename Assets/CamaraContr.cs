using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public partial class CamaraContr : MonoBehaviour
{
    [SerializeField] Transform objetivo;
}
public partial class CamaraContr : MonoBehaviour
{
    private void Update()
    {
        if (objetivo != null)
        {
            transform.position = new Vector3(
                objetivo.position.x,
                objetivo.position.y,
                -10
            );
        }
    }
}
