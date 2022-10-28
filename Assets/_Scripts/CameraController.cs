using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed;
    [SerializeField] float maxCameraSpeed;
    [SerializeField] float minCameraSpeed;

    public float difficultyFactor = 1;

    public float speedFactor = 1f;

    public bool isSpeedUp;

    private void Start()
    {
        minCameraSpeed = cameraSpeed;
        maxCameraSpeed = cameraSpeed * 2f;
    }
    private void Update()
    {
        Movement();
    }


    void Movement()
    {
        if (!isSpeedUp)
        {
            cameraSpeed = Mathf.Lerp(cameraSpeed, minCameraSpeed, 0.5f);
            transform.Translate(Vector3.forward * cameraSpeed * difficultyFactor * Time.deltaTime, Space.World);
            SpeedFactor(1);
        }
        else
        {
            cameraSpeed = Mathf.Lerp(cameraSpeed, maxCameraSpeed, 0.5f);
            transform.Translate(Vector3.forward * cameraSpeed * difficultyFactor * Time.deltaTime, Space.World);
            SpeedFactor(2);
        }

    }


    public void Speed()
    {
        isSpeedUp = !isSpeedUp;
    }



    float SpeedFactor(float value)
    {
        speedFactor = value;
        return speedFactor;
    }

}
