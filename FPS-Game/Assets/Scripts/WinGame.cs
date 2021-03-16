using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public Transform player;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform == player)
        {
            Debug.Log("You win :)");
        }
        
    }
}
