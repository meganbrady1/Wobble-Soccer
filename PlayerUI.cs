using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    #region Public Fields

    [Tooltip("Pixel offset from the player target")]
    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    #endregion

    #region Private Fields


    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;


    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    private PlayerManager1 target;

    float characterControllerHeight = 0f;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup _canvasGroup;
    Vector3 targetPosition;


    #endregion


    #region MonoBehaviour Callbacks

    void Awake()
    {
        /*
         * Why going brute force and find the Canvas this way?
         * Because when scenes are going to be loaded and unloaded, 
         * so is our Prefab, and the Canvas will be everytime different. 
         * To avoid more complex code structure, we'll go for the quickest way. 
         * However it's really not recommended to use "Find", because this is a slow 
         * operation. This is out of scope for this tutorial to implement a more 
         * complex handling of such case, but a good exercise when you'll feel comfortable
         * with Unity and scripting to find ways into coding a better management 
         * of the reference of the Canvas element that takes loading and unloading 
         * into account.
         */
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        _canvasGroup = this.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Reflect the Player Health
        //if (playerHealthSlider != null)
        //{
        //    playerHealthSlider.value = target.Health;
        //}

        //// Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        //if (target == null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
    }

    void LateUpdate()
    {
        // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
        if (targetRenderer != null)
        {
            this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
        }


        // #Critical
        // Follow the Target GameObject on screen.
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
    #endregion


    #region Public Methods

    public void SetTarget(PlayerManager1 _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency
        target = _target;
        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponent<Renderer>();
        CharacterController characterController = _target.GetComponent<CharacterController>();
        // Get data from the Player that won't change during the lifetime of this Component
        if (characterController != null)
        {
            characterControllerHeight = characterController.height;
        }
        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }
    #endregion
}
