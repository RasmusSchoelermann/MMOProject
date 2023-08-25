using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using System;
using Cinemachine;

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
    public CinemachineFreeLook cinemachineCameraSettings;

    public Vector2 movementInput;
    public Vector2 mouseInput = Vector2.zero;
    public Vector2 mousePosition = Vector2.zero;
    public float rotationInput;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float mouseSensitivity = 3.0f;

    public bool mouse0;
    public bool mouse1;

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

        inputManager.MouseInput.Mouse0.performed += ctx => LeftMouseAction();
        inputManager.MouseInput.Mouse0.canceled += ctx => mouse0 = false;

        inputManager.MouseInput.Mouse1.started += ctx => RightMouseAction();
        inputManager.MouseInput.Mouse1.canceled += ctx => mouse1 = false;

        inputManager.MouseInput.MouseDelta.performed += ctx => MouseInput(ctx.ReadValue<Vector2>());
        //inputManager.MouseInput.MouseDelta.canceled += ctx => MouseInput(mouseInput = new Vector2(0, 0));
        inputManager.MouseInput.MousePosition.performed += ctx => MousePosition(ctx.ReadValue<Vector2>());
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
        if (!IsOwner)
            return;
        cameraMovement.ApplyCameraRotation();
        //MouseInput();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        playerMovement.CheckPlayerMovement();
        playerRotation.CheckRotationInput();

        cameraMovement.CharacterViewDirection();
    }

    private void LeftMouseAction()
    {
        mouse0 = true;

        if(rotationInput != 0)
        {
            playerMovement.ApplySidewardsMovementAD();
        }
    }

    private void RightMouseAction()
    {
        mouse1 = true;

        if (mouse0)
        {
            playerMovement.ApplyForwardsMovementWithMouse();
        }
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
        mouseInput = mInput;
    }

    private void MousePosition(Vector2 mPos)
    {
        mPos.Normalize();
        mousePosition = mPos;
    }

    public void SetCameraDamping(float value)
    {
        for (int i = 0; i < 3; i++)
        {
            CinemachineOrbitalTransposer transposer = cinemachineCameraSettings.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>();
            transposer.m_XDamping = value;
        }
    }

    public bool Mouse0And1Pressed()
    {
        if(mouse0 && mouse1)
            return true;

        return false;
    }

    public bool Mouse0Pressed()
    {
        if (mouse0 && !mouse1)
            return true;

        return false;
    }

    public bool Mouse1Pressed()
    {
        if(!mouse0 && mouse1)
            return true;

        return false;
    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.47f, playerCollider.bounds.center.z), playerCollider.radius, groundLayer);
    }
}
