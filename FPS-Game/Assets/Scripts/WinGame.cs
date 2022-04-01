using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public Transform player;

    public GameObject winMenu;

    public MouseLook mouselook;
    public GunScript gunScript;
    public newPlayerMovement playerMovement;

    public Timer timer;
    public Transform winTimer;

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
            playerMovement.enabled = false;
                
            timer.stopPlaying();

            winTimer.GetComponent<TextMeshProUGUI>().text = "Time: " + timer.GameTime;
        }
        
    }
}
