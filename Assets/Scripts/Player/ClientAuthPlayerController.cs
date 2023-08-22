using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using System;

public class ClientAuthPlayerController : NetworkBehaviour
{
    [SerializeField]
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
        _mouseInput.x = Input.GetAxis("Mouse X") * MouseSensitivity;
        _mouseInput.y = Input.GetAxis("Mouse Y") * MouseSensitivity;


    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.47f, playerCollider.bounds.center.z), playerCollider.radius, groundLayer);
    }
}
