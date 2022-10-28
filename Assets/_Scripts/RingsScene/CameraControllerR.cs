using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerR : MonoBehaviour
{
    public GameObject ring;
    void Start()
    {
        transform.LookAt(ring.transform);
    }

    

}
