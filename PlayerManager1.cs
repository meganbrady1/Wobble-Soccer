using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerManager1 : MonoBehaviourPunCallbacks, IPunObservable

{
    #region Public Fields
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    //public Animator animator;
    public float speed;
    public float strafeSpeed;

    public float jumpForce;

    public Rigidbody hips;
    public bool isGrounded;
    #endregion
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{

        //    //We own this player: send the others our data
        //    stream.SendNext(IsFiring);
        //    stream.SendNext(Health);
        //}
        //else
        //{
        //    //Network player, reveive data
        //    this.IsFiring = (bool)stream.ReceiveNext();
        //    this.Health = (float)stream.ReceiveNext();
        //}
    }

    #endregion



    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager1.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (photonView.IsMine) { hips = GetComponent<Rigidbody>(); }
        
        /*Photon.Pun.Demo.PunBasics.CameraWork _cameraWork = this.gameObject.GetComponent<Photon.Pun.Demo.PunBasics.CameraWork>();

        //CameraControl _cameraControl = camera.GetComponent<CameraControl>();
        //CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }*/

#if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
    }

#if !UNITY_5_4_OR_NEWER
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif


    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

#if UNITY_5_4_OR_NEWER
    public override void OnDisable()
    {
        // Always call the base to remove callbacks
        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
#endif
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void FixedUpdate()
    {

        if (photonView.IsMine)
        {
            ProcessInputs();
        }
    }

    #endregion

    #region Custom

    /// <summary>
    /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
    /// </summary>
    void ProcessInputs()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    if (!IsFiring)
        //    {
        //        IsFiring = true;
        //    }
        //}
        //if (Input.GetButtonUp("Fire1"))
        //{
        //    if (IsFiring)
        //    {
        //        IsFiring = false;
        //    }
        //}
        //Sprinting if statement
        // _ = Input.GetKey(KeyCode.LeftShift) ? speed = defspeed * 1.5f: speed = defspeed;

        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //hips.AddForce(hips.transform.forward * speed * 1.5f);
                hips.AddForce(hips.transform.right * speed * 1.5f);
            }
            else
            {
                //hips.AddForce(hips.transform.forward * speed);
                hips.AddForce(hips.transform.right * speed);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift)){
                //hips.AddForce(-hips.transform.right * speed * 1.5f);
                hips.AddForce(hips.transform.forward * speed * 1.5f);
            }
            else{
                //hips.AddForce(-hips.transform.right * speed);
                hips.AddForce(hips.transform.forward * speed);
            }
        }


        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift)){
                hips.AddForce(-hips.transform.right * speed * 1.5f);
                //hips.AddForce(-hips.transform.forward * speed * 1.5f);
            }
            else{
                //hips.AddForce(-hips.transform.forward * speed);
                hips.AddForce(-hips.transform.right * speed);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if(Input.GetKey(KeyCode.LeftShift)){
                //hips.AddForce(hips.transform.right * speed * 1.5f);
                hips.AddForce(-hips.transform.forward * speed * 1.5f);
            }
            else{
                //hips.AddForce(hips.transform.right * speed);
                hips.AddForce(-hips.transform.forward * speed);
            }
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                hips.AddForce(new Vector3(0, jumpForce, 0));
                isGrounded = false;
            }
        }
    }

    #endregion

    #region PrivateMethods

#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
#endif

    #endregion
}