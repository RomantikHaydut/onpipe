using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RingControllerR : MonoBehaviour
{
    MeshRenderer meshRenderer;

    public Material yellowMaterial;
    public Material blackMaterial;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (DataManager.Instance.isRingYellow)
        {
            MakeYellow();
        }
        else if (DataManager.Instance.isRingBlack)
        {
            MakeBlack();
        }
        else
        {
            MakeYellow();
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 90 * Time.deltaTime , Space.World);
    }

    public void MakeYellow()
    {
        meshRenderer.material = yellowMaterial;
        DataManager.Instance.isRingYellow = true;
        DataManager.Instance.isRingBlack = false;
        DataManager.Instance.SaveMaterial();
    }

    public void MakeBlack()
    {
        meshRenderer.material = blackMaterial;
        DataManager.Instance.isRingYellow = false;
        DataManager.Instance.isRingBlack = true;
        DataManager.Instance.SaveMaterial();
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
