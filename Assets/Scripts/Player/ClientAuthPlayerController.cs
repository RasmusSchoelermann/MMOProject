using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using System;

public class ClientAuthPlayerController : NetworkBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerRotation playerRotation;
    [SerializeField]
    private CameraMovement cameraMovement;

    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCollider;

    public Transform player;
    public Transform playerObject;
    public Transform orientation;

    public Camera playerCam;

    public Vector2 movementInput;
    public Vector2 mouseInput = Vector2.zero;
    public float rotationInput;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float mouseSensitivity = 3.0f;

    private bool mouse0;

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set
        {
            if (value > 30)
                throw new ArgumentException("Movementspeed Cap exceeded");
            movementSpeed = value;
        }
    }

    public float JumpForce
    {
        get { return jumpForce; }
        set
        {
            if (value > 10)
                throw new ArgumentException("Jump force cap exceeded");
            jumpForce = value;
        }
    }

    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set
        {
            if (value > 200)
                throw new ArgumentException("Rotation Speed cap exceeded");
            rotationSpeed = value;
        }
    }

    public float MouseSensitivity
    {
        get { return mouseSensitivity; }
        set { mouseSensitivity = value; }
    }

    public bool Mouse0
    {
        get { return mouse0; }
        set { mouse0 = value; }
    }

    [SerializeField]
    private LayerMask groundLayer;
    

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        if (!IsOwner)
            return;

        playerMovement.InitializePlayerMovement(this, playerRotation, cameraMovement);
        playerRotation.InitializePlayerRotation(this);
        cameraMovement.InitializeCamera(this, playerMovement, playerRotation);
    }

    private void Awake()
    {
        SetupInput();
    }

    private void SetupInput()
    {
        inputManager = new InputManager();

        inputManager.Movement.Movement.performed += ctx => StandardMovement(ctx.ReadValue<Vector2>());
        inputManager.Movement.Movement.canceled += ctx => StandardMovement(Vector2.zero);

        inputManager.Movement.Rotation.performed += ctx => Rotation(ctx.ReadValue<float>());
        inputManager.Movement.Rotation.canceled += ctx => Rotation(0.0f);

        inputManager.Movement.Jump.performed += ctx => playerMovement.ApplyJump();

        inputManager.MouseInput.Mouse0.performed += ctx => Mouse0 = true;
        inputManager.MouseInput.Mouse0.canceled += ctx => Mouse0 = false;

        inputManager.MouseInput.MouseDelta.performed += ctx => MouseInput(ctx.ReadValue<Vector2>());
        //inputManager.MouseInput.MouseDelta.canceled += ctx => MouseInput(mouseInput = new Vector2(0, 0));

    }

    private void OnEnable()
    {
        inputManager.Enable();
    }

    private void OnDisable()
    {
        inputManager.Disable();
    }

    private void Update()
    {
        CharacterViewDirection();
        //MouseInput();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        playerMovement.CheckPlayerMovement();
        playerRotation.ApplyRotation();

        cameraMovement.UpdateCamera();
        //cameraMovement.FocusCamOnPlayer(player);
    }

    private void CharacterViewDirection()
    {
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        if(viewDirection != Vector3.zero)
            orientation.forward = viewDirection.normalized;
    }

    private void StandardMovement(Vector2 moveInput)
    {
        movementInput = moveInput;
    }

    private void Rotation(float rotation)
    {
        rotationInput = rotation;
    }

    private void MouseInput(Vector2 mInput)
    {
        mInput.Normalize();
        //mInput.x = -mInput.x;
        //mInput.y = -mInput.y;
        mouseInput = mInput;
        Debug.Log("x: " + mInput.x + " y: " + mInput.y);
        //mouseInput.x = mInput.y;
        //mouseInput.y = mInput.x;
    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.47f, playerCollider.bounds.center.z), playerCollider.radius, groundLayer);
    }



    /*[SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float mouseSensitivity = 3.0f;

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set
        {
            if (value > 30)
                throw new ArgumentException("Movementspeed Cap exceeded");
            movementSpeed = value;
        }
    }

    public float JumpForce
    {
        get { return jumpForce; }
        set { if (value > 10)
                throw new ArgumentException("Jump force cap exceeded");
            jumpForce = value;
        }
    }

    public float RotationSpeed
    {
        get { return  rotationSpeed; }
        set { if (value > 200)
                throw new ArgumentException("Rotation Speed cap exceeded");
        rotationSpeed = value;
        }
    }

    private float MouseSensitivity
    {
        get { return mouseSensitivity; }
        set { mouseSensitivity = value; }
    }

    [SerializeField]
    private Transform playerBody;

    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCollider;

    public Vector2 _movementInput;
    public float _rotationInput;
    public Vector2 _mouseInput;

    [SerializeField]
    private LayerMask groundLayer;

    public Transform playerTransform;

    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerRotation playerRotation;
    [SerializeField]
    private CameraMovement cameraMovement;

    public Camera playerCam;

    public Vector3 testVelocity;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        if (!IsOwner)
            return;

        if (playerRotation == null || playerMovement == null || cameraMovement == null)
            return;

        playerMovement.InitializePlayerMovement(this, playerRotation, cameraMovement);
        playerRotation.InitializePlayerRotation(this);
        cameraMovement.InitializeCamera(this, playerMovement, playerRotation);
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if(Input.GetKey(KeyCode.R))
        {
            playerRotation.RotatePlayerInCameraDirection();
        }

        testVelocity = playerRigidbody.velocity;

        MovementInput();
        MouseInput();
        playerRotation.ApplyRotation();

        cameraMovement.ZoomCamera();
        cameraMovement.ManualCameraMovement();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        playerMovement.ApplyMovement();
        cameraMovement.PlayerCameraMovement();
    }

    private void MovementInput()
    {
        _movementInput.x = Input.GetAxisRaw("Sidewards");
        _movementInput.y = Input.GetAxisRaw("Forwards");
        _rotationInput = Input.GetAxisRaw("Rotation");

        if(Input.GetButtonDown("Jump"))
        {
            playerMovement.ApplyJump();
        }
    }

    private void MouseInput()
    {
        if (Input.GetMouseButton(1))
        {
            _mouseInput.x = Input.GetAxis("Mouse X") * MouseSensitivity;
            _mouseInput.y = Input.GetAxis("Mouse Y") * MouseSensitivity;
        }


    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.47f, playerCollider.bounds.center.z), playerCollider.radius, groundLayer);
    }*/
}
