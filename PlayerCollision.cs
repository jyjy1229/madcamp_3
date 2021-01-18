using UnityEngine;
using Photon.Pun;

public class PlayerCollision : MonoBehaviourPun
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("@@@@@@@@@@@@@@@@@@@");
            gameObject.GetComponent<Rigidbody>().AddForce(collision.relativeVelocity);
        }
    }
}