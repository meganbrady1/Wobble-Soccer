using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class Ballscript : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Public Fields

    [SerializeField]
    public GameObject BallPrefab;
    public Text score;
    public Rigidbody ball;

    public int redScore = 0;
    public int blueScore = 0;


    #endregion
    #region MonoBehavior Callbacks

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{

        //    //We own this player: send the others our data
        //    Debug.Log($"Sending the score: '{score}'");
        //    stream.SendNext(score.text);
        //}
        //else
        //{
        //    //Network player, reveive data
        //    Debug.Log($"receiving the score: '{score}'");
        //    this.score.text = (string)stream.ReceiveNext();
        //}
    }

    #endregion
    void Start()
    {
        redScore = 0;
        blueScore = 0;
        score.text = "0 - 0";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Red Net")
        {
            redScore += 1;
            
            this.transform.position = new Vector3(0f, 4f, 0f);
            ball.isKinematic = true;
            ball.isKinematic = false;
            UpdateScore(redScore, blueScore);
        }else if (collision.gameObject.tag == "Blue Net")
        {
            blueScore += 1;
            this.transform.position = new Vector3(0f,4f,0f);
            ball.isKinematic = true;
            ball.isKinematic = false;
            UpdateScore(redScore, blueScore);
        }
        if (collision.gameObject)
        {
            Debug.Log(collision.gameObject);
        }
    }
    #endregion
    #region Custom Functions
    private void UpdateScore(int r, int b)
    {
        score.text = $"{b} - {r}";
    }
    #endregion

}
