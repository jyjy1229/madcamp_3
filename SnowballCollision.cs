using UnityEngine;
using Photon.Pun;

public class SnowballCollision : MonoBehaviourPun
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.rigidbody.AddForce(collision.relativeVelocity);
            Destroy(gameObject);
        }
    }
}