using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectController : MonoBehaviour
{
    float turnSpeed = 0.75f;

    float maxTurnSpeed = 2.0f;

    float minTurnSpeed = 0.75f;

    bool collided = false;

    Rigidbody rb;

    public Color startColor;

    public Color collectableColor;

    PlayerManager playerManager;

    MeshRenderer[] renderers;

    List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    List<Color> colors = new List<Color>();

    Vector3 turnDir;

    Vector3 throwDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        startColor = GetComponent<MeshRenderer>().material.color;

        playerManager = FindObjectOfType<PlayerManager>();

        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //renderers[i] = transform.GetChild(0).transform.GetChild(i).GetComponent<MeshRenderer>();
            meshRenderers.Add(transform.GetChild(0).transform.GetChild(i).GetComponent<MeshRenderer>());
            colors.Add(transform.GetChild(0).transform.GetChild(i).GetComponent<MeshRenderer>().material.color);
        }

        turnDir = (transform.position - transform.parent.transform.position).normalized;

    }
    void Update()
    {
        
        if (playerManager.canCollect && playerManager.activeCylinder.transform == transform.parent && !collided && playerManager.gameObject.transform.position.z < (transform.position.z + transform.localScale.x / 2))
        {
            turnSpeed = Mathf.Lerp(turnSpeed, maxTurnSpeed, 0.2f);
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Color color = Color.Lerp(meshRenderers[i].material.color, collectableColor, 0.3f);
                meshRenderers[i].material.color = color;
            }
            
        }
        else
        {
            turnSpeed = Mathf.Lerp(turnSpeed, minTurnSpeed, 0.2f);
            if (!collided)
            {
                for (int i = 0; i < meshRenderers.Count; i++)
                {
                    Color color = Color.Lerp(meshRenderers[i].material.color, colors[i], 0.3f);
                    meshRenderers[i].material.color = color;
                }
            }
            else
            {
                for (int i = 0; i < meshRenderers.Count; i++)
                {
                    Color color = Color.Lerp(meshRenderers[i].material.color, Color.black, 1f);
                    meshRenderers[i].material.color = color;
                }
            }
            
        }

        if (collided)
        {
            if (transform.localScale.x > 0.05f)
            {
                transform.Rotate(transform.up * 360 * Time.deltaTime * turnSpeed, Space.Self);
                transform.localScale -= transform.localScale * Time.deltaTime * 1.5f;
                transform.position += throwDir * Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.zero;
                enabled = false;
            }
            
        }
        

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!collided)
            {  
                if (Mathf.Abs(Vector3.Distance(transform.position, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.position.z)) + transform.localScale.x) >= Mathf.Abs(other.gameObject.GetComponent<PlayerManager>().radius))
                {
                    
                    Vector3 forceWay = ((transform.parent.position + new Vector3(0, 0, transform.parent.localScale.y)) - transform.position).normalized;

                    //rb.AddForce(new Vector3(-forceWay.x, -forceWay.y, forceWay.z) * 6f, ForceMode.Impulse);

                    throwDir = new Vector3(-forceWay.x, -forceWay.y, forceWay.z).normalized * 6f;

                    CylinderController cylinderController = transform.parent.GetComponent<CylinderController>();
                    if (cylinderController != null)
                    {
                        if (cylinderController.enabled)
                        {
                            cylinderController.isAddedHealth = true;
                        }
                    }
                   // playerManager.AddHealth((15f / transform.parent.childCount) * 10);
                    collided = true;
                }
                Destroy(gameObject, 4f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!collided)
            {
                if (transform.position.z > other.gameObject.transform.position.z)
                {
                    if (Mathf.Abs(Vector3.Distance(transform.position, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.position.z)) + transform.localScale.x) >= Mathf.Abs(other.gameObject.GetComponent<PlayerManager>().radius))
                    {
                        CylinderController cylinderController = transform.parent.GetComponent<CylinderController>();
                        if (cylinderController != null)
                        {
                            if (cylinderController.enabled)
                            {
                                cylinderController.isAddedHealth = true;
                            }
                        }
                        collided = true;
                        Vector3 forceWay = (((transform.parent.position + new Vector3(0, 0, transform.parent.localScale.y)) + Vector3.forward) - transform.position).normalized;
                        throwDir = new Vector3(-forceWay.x, -forceWay.y, forceWay.z).normalized * 6f;
                        transform.parent = null;
                       // rb.AddForce(new Vector3(-forceWay.x, -forceWay.y, forceWay.z) * 10f, ForceMode.Impulse);
                    }
                    Destroy(gameObject, 4f);
                }

            }
        }
    }
}
