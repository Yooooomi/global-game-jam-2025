using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestroy : MonoBehaviour
{
    [SerializeField]
    private float destroyIn;

    void Start()
    {
        Destroy(transform.gameObject, destroyIn);
    }
}
