using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;

public class ClientAuthPlayerController : NetworkBehaviour
{
    public float movementSpeed;
    public float jumpForce;
    public float rotationSpeed;

    public bool isJumping;

    public Transform playerTransform;
    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCollider;

    public Vector2 _playerInput;
    public Vector3 newVelocity;
    public Vector3 newForce;

    public LayerMask groundLayer;

    public PlayerMovement playerMovement;
    public PlayerRotation playerRotation;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        if (playerRotation == null && playerMovement == null)
            return;

        playerMovement.InitializePlayerMovement(this, playerRotation);
        playerRotation.InitializePlayerRotation(this);
    }

    private void Update()
    {
        MovementInput();
        playerRotation.ApplyRotation();
    }

    private void FixedUpdate()
    {
        playerMovement.ApplyMovement();
    }

    private void MovementInput()
    {
        _playerInput.x = Input.GetAxisRaw("Horizontal");
        _playerInput.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            playerMovement.ApplyJump();
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.47f, playerCollider.bounds.center.z), playerCollider.radius, groundLayer);
    }
}
