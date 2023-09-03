using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientAuthPlayerController : NetworkBehaviour
{
    private InputManager inputManager;
    private PlayerMovement playerMovement;
    private PlayerRotation playerRotation;
    private CameraMovement cameraMovement;
    private PlayerInteraction playerInteraction;
    private PlayerCombat playerCombat;

    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCollider;

    public Transform player;
    public Transform playerObject;
    public Transform orientation;

    public Camera playerCam;

    public Vector2 movementInput;
    public Vector2 mouseInput = Vector2.zero;
    public Vector2 mousePosition = Vector2.zero;
    public float rotationInput;

    public bool mouse0;
    public bool mouse1;

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

        playerMovement.InitializeMovement();
        playerRotation.InitializePlayerRotation(this, cameraMovement);
        cameraMovement.InitializeCamera(this);
        playerInteraction.InitializePlayerInteraction(this, playerCombat);

    }

    private void Awake()
    {
        SetupRefs();
        SetupInput();
    }

    private void SetupRefs()
    {
        if(gameObject.GetComponent<PlayerMovement>() != null)
            playerMovement = GetComponent<PlayerMovement>();

        if(gameObject.GetComponent<PlayerRotation>() != null)
            playerRotation = GetComponent<PlayerRotation>();

        if(gameObject.GetComponentInChildren<CameraMovement>() != null)
            cameraMovement = GetComponentInChildren<CameraMovement>();

        if(gameObject.GetComponent<PlayerInteraction>() != null)
            playerInteraction = GetComponent<PlayerInteraction>();
        
        if(gameObject.GetComponent<PlayerCombat>() != null)
            playerCombat = GetComponent<PlayerCombat>();
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

        inputManager.MouseInput.MousePosition.performed += ctx => MousePosition(ctx.ReadValue<Vector2>());

        inputManager.MouseInput.Zoom.performed += ctx => MouseZoom(ctx.ReadValue<Vector2>().normalized);
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

        EventManager.OnMouseClickRayCast.Invoke(mousePosition);

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
        mousePosition = mPos;
    }

    private void MouseZoom(Vector2 mZoom)
    {
        cameraMovement.ZoomAmount += mZoom.y;
        cameraMovement.ZoomCamera();
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
