using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderController : MonoBehaviour
{
    PlayerManager playerManager;

    public bool isAddedHealth = false;
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (isAddedHealth)
        {
            FindObjectOfType<PlayerManager>().AddHealth(13);
            playerManager.OrcSound();
            enabled = false;
        }
    }


}
