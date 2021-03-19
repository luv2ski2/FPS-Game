using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public Transform player;

    public GameObject winMenu;

    public MouseLook mouselook;
    public GunScript gunScript;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform == player)
        {
            Debug.Log("You win :)");
            winMenu.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            mouselook.enabled = false;
            gunScript.enabled = false;
        }
        
    }
}
