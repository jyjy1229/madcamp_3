using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
   public Rigidbody Bullet;
    public Transform FirePos;
    public Rigidbody clone;
    bool hi = true;

    void Start(){

    }
 
    void Update () {

        if (Input.GetMouseButtonUp (0))
        {
            GameObject clone = (GameObject)Instantiate(Bullet.gameObject, FirePos.transform.position, FirePos.transform.rotation);
            //clone.transform.localScale = new Vector3(1f,1f,1f);
        }
        if (Input.GetMouseButtonDown(0)){
            //clone.transform.localScale += new Vector3(0.5f,0.5f,0.5f);
        }
    }


}
