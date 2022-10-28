using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject cylinder;

    GameObject previousCylinder;

    GameObject lastCylinder;

    [SerializeField] Color enemyCylinderColor;

    private float pointSpawnRate = 2f;
    private float pointSpawnRateStart;

    private int cylinderCount;

    private const int wantedCylinderCount = 200;

    private const int wantedCylinderCountInScene = 35;

    GameObject player;

    public GameObject pointObject;

    List<GameObject> cylinderList = new List<GameObject>();

    GameObject[] activeCylinders;

    private void Start()
    {
        pointSpawnRateStart = pointSpawnRate;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < wantedCylinderCount; i++)
        {

            float height = Random.Range(1.4f, 3.5f);

            float radius_ = FindRadius();

            cylinder.transform.localScale = new Vector3(radius_, height, radius_);

            //Instantiate first cylinder.
            if (i == 0)
            {
                previousCylinder = Instantiate(cylinder, Vector3.zero, Quaternion.identity);
            }

            //Instantiate other cylinders.
            else
            {
                float spawnPoint = previousCylinder.transform.position.z;
                previousCylinder = Instantiate(cylinder, new Vector3(0, 0, spawnPoint + previousCylinder.transform.localScale.y + cylinder.transform.localScale.y), Quaternion.identity);

                // Make some cylinders enemy.
                if (Random.value <= 0.1f && previousCylinder.GetComponent<Renderer>().material.color != enemyCylinderColor)
                {
                    previousCylinder.GetComponent<Renderer>().material.color = enemyCylinderColor;
                    previousCylinder.tag = "Enemy";
                }


            }

            // Rotate cylinders.
            previousCylinder.transform.Rotate(90, 0, 0);
            cylinderList.Add(previousCylinder);
            
        }

        for (int i = cylinderList.Count - 1; i >= wantedCylinderCountInScene; i--)
        {
            if (i != wantedCylinderCountInScene)
            {
                cylinderList[i].gameObject.SetActive(false);
            }
            else
            {
                lastCylinder = cylinderList[i].gameObject; 
            }
        }

        StartCoroutine(SpawnPoints());


    }

    private void Update()
    {
        cylinderCount = GameObject.FindGameObjectsWithTag("Cylinder").Length + GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (cylinderCount < wantedCylinderCountInScene)
        {
            if (GameManager.Instance.isPlayerAlive)
            {
                MoveCylinder();
            }
            
        }

    }

    float FindRadius()
    {
        float radius = Random.Range(1.2f, 4f);
        if (previousCylinder != null)
        {
            while (Mathf.Abs(radius - previousCylinder.transform.localScale.x) <= 0.6f)
            {
                radius = Random.Range(1.2f, 4f);
            }
        }
        return radius;
    }

    void MoveCylinder()
    {
        int randomIndex = Random.Range(0, cylinderList.Count-1);

        if (cylinderList[randomIndex].gameObject.activeInHierarchy)
        {
            MoveCylinder();
        }
        else
        {
            if (Mathf.Abs(cylinderList[randomIndex].gameObject.transform.localScale.z - lastCylinder.transform.localScale.z) <= 0.6f)
            {
                MoveCylinder();
            }
            else
            {
                Vector3 movePosition = lastCylinder.transform.position + new Vector3(0, 0, (lastCylinder.transform.localScale.y) + cylinderList[randomIndex].gameObject.transform.localScale.y);
                cylinderList[randomIndex].gameObject.transform.position = movePosition;
                lastCylinder = cylinderList[randomIndex].gameObject;
                cylinderList[randomIndex].gameObject.SetActive(true);
            }
            
        }

        
    }

    IEnumerator SpawnPoints()
    {
        while (true)
        {
            pointSpawnRate = pointSpawnRateStart / (FindObjectOfType<CameraController>().difficultyFactor);
            yield return new WaitForSecondsRealtime(pointSpawnRate);
            Collider[] cylinders = Physics.OverlapSphere(player.transform.position + new Vector3(0, 0, 12), 0.002f, LayerMask.NameToLayer("cylinder"));

            if (cylinders.Length == 1 && cylinders[0].transform.childCount == 0 && !cylinders[0].transform.CompareTag("Enemy"))
            {

                int pointCount = (int)cylinders[0].gameObject.transform.localScale.z * 6;

                float randomZ = Random.Range((-cylinders[0].gameObject.transform.localScale.z / 2) + transform.localScale.x / 2, (cylinders[0].gameObject.transform.localScale.z / 2) - transform.localScale.x / 2);
                List<GameObject> pointObjectList = new List<GameObject>();
                for (int i = 0; i < pointCount; i++)
                {
                    float radius = cylinders[0].gameObject.transform.localScale.z / 2;
                    float angle = (i * 2 * Mathf.PI) / pointCount;

                    Vector3 spawnPos = cylinders[0].gameObject.transform.position + new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
                    GameObject pointObjectClone = Instantiate(pointObject, spawnPos, pointObject.transform.rotation);

                    pointObjectClone.transform.position += new Vector3(0, 0, randomZ);
                    pointObjectClone.transform.LookAt(transform);
                    //pointObjectClone.transform.parent = cylinders[0].transform;
                    pointObjectList.Add(pointObjectClone);
                }

                for (int i = 0; i < pointObjectList.Count; i++)
                {
                    pointObjectList[i].transform.parent = cylinders[0].transform;
                }
            }
            else if (cylinders.Length > 1)
            {
                Debug.LogWarning("There are more than one cylinders at spawn point !!!");
            }

        }
    }
}
