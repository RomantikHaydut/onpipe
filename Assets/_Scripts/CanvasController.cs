using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    float factor;
    CanvasScaler canvasScaler;
    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    void ChangeCanvasScale()
    {
        canvasScaler.scaleFactor = factor;
    }

}
