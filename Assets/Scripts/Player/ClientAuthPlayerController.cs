using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;

public class ClientAuthPlayerController : NetworkBehaviour
{
    public float _movementSpeed;
    public float _jumpForce;
    public float _rotationSpeed;
    public float _mouseSensititvity = 3.0f;

    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCollider;

    public Vector2 _movementInput;
    public Vector2 _mouseInput;

    public Vector3 _newVelocity;
    public Vector3 _newForce;

    public LayerMask groundLayer;

    public Transform playerTransform;
    public PlayerMovement playerMovement;
    public PlayerRotation playerRotation;
    public CameraMovement cameraMovement;

    public Camera playerCam;

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
        cameraMovement.InitializeCamera(this, playerMovement);
    }

    private void Update()
    {
        if (!IsOwner)
            return;

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
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            playerMovement.ApplyJump();
        }
    }

    private void MouseInput()
    {
        _mouseInput.x = Input.GetAxis("Mouse X") * _mouseSensititvity;
        _mouseInput.y = Input.GetAxis("Mouse Y") * _mouseSensititvity;


    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.47f, playerCollider.bounds.center.z), playerCollider.radius, groundLayer);
    }
}
