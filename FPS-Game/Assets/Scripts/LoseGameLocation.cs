using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGameLocation : MonoBehaviour
{
    public Transform player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform == player)
        {
            player.GetComponent<newPlayerMovement>().GetHit();
        }
    }
}
