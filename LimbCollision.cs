using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    public PlayerManager1 playerManager1;

    private void Start() 
    {
        playerManager1 = GameObject.FindObjectOfType<PlayerManager1>().GetComponent<PlayerManager1>();
    }

    private void OnCollisionEnter(Collision collision) 
    {
        _ =collision.gameObject.tag == "ground" ? playerManager1.isGrounded = true:false;
    }
}
