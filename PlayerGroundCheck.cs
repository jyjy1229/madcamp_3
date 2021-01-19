using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    MyPlayer myPlayer;
    
    void Awake()
    {
        myPlayer = GetComponentInParent<MyPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == myPlayer.gameObject)
            return;

        myPlayer.SetGroundedState(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == myPlayer.gameObject)
            return;

        myPlayer.SetGroundedState(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == myPlayer.gameObject)
            return;

        myPlayer.SetGroundedState(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == myPlayer.gameObject)
            return;

        myPlayer.SetGroundedState(true);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == myPlayer.gameObject)
            return;

        myPlayer.SetGroundedState(false);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == myPlayer.gameObject)
            return;
        
        myPlayer.SetGroundedState(true);
    }
}
