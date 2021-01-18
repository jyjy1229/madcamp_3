using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public Rigidbody bullet;
  public GameObject capsule;
  bool bv = true;


  void FixedUpdate () {

    if (Input.GetMouseButtonDown(0)){
      //bullet.transform.localScale += new Vector3(1f,1f,1f);
    }
        //프레임마다 오브젝트를 로컬좌표상에서 앞으로 1의 힘만큼 날아가라
      bullet.AddRelativeForce(Vector3.right*40f);

  }

  void OnCollisionEnter(Collision collision){
    if(collision.gameObject.tag == "wall")
      Destroy(bullet.gameObject);

    if(collision.gameObject.tag == "Player")
      collision.rigidbody.AddForce(collision.relativeVelocity);
  }
}
