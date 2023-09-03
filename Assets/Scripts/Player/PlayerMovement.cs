using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerRotation playerRotation;

    private Vector3 _newVelocity;
    private Vector3 _newJumpForce;

    private Vector3 _moveDirection;

    [SerializeField]
    private float _groundDrag;

    [SerializeField]
    private float movementSpeed = 10;

    [SerializeField]
    private float jumpForce = 5;

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

    public void InitializeMovement()
    {
        clientController = GetComponent<ClientAuthPlayerController>();
        playerRotation = GetComponent<PlayerRotation>();
    }

    public void CheckPlayerMovement()
    {
        VelocityControl();

        if (!clientController.IsGrounded())
        {
            FallingControls();
            clientController.playerRigidbody.drag = 0f;
            return;
        }

        clientController.playerRigidbody.drag = _groundDrag;

        if (clientController.Mouse0And1Pressed())
            ApplyForwardsMovementWithMouse();

        if (clientController.Mouse1Pressed() && clientController.rotationInput != 0)
            ApplySidewardsMovementAD();

        if (clientController.movementInput != Vector2.zero)
            ApplyMovement();


    }

    private void ApplyMovement()
    {
        if (clientController.movementInput.x == 0)
        {
            EventManager.OnCamDampChanged.Invoke(1f);
        }
        else
        {
            EventManager.OnCamDampChanged.Invoke(0f);
        }

        _moveDirection = clientController.orientation.forward * clientController.movementInput.y + clientController.orientation.right * clientController.movementInput.x;

        clientController.playerRigidbody.AddForce(_moveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);

        if( clientController.rotationInput != 0)
        {
            playerRotation.RotatePlayerOnPoint();
        }

    }

    public void ApplySidewardsMovementAD()
    {
        EventManager.OnCamDampChanged.Invoke(0f);
        _moveDirection = clientController.orientation.forward * clientController.movementInput.y + clientController.orientation.right * clientController.rotationInput;

        clientController.playerRigidbody.AddForce(_moveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);
        //clientController.playerObject.forward = Vector3.Slerp(clientController.orientation.forward, clientController.orientation.right * clientController.rotationInput, Time.deltaTime * clientController.RotationSpeed);
    }

    public void ApplyForwardsMovementWithMouse()
    {
        EventManager.OnCamDampChanged.Invoke(0f);
        _moveDirection = clientController.orientation.forward * 1.0f;

        clientController.playerRigidbody.AddForce(_moveDirection.normalized * MovementSpeed * 10f, ForceMode.Force);
        clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, clientController.orientation.forward, Time.deltaTime * playerRotation.RotationSpeed);
    }


    private void FallingControls()
    {
        _newVelocity.Set(clientController.movementInput.x * MovementSpeed / 2, clientController.playerRigidbody.velocity.y, clientController.movementInput.y * MovementSpeed / 2);
        clientController.playerRigidbody.AddRelativeForce(new Vector3(_newVelocity.x, _newVelocity.y, _newVelocity.z));
    }

    public void ApplyJump()
    {
        if (clientController.IsGrounded())
        {
            _newJumpForce.Set(0.0f, JumpForce, 0.0f);
            clientController.playerRigidbody.AddForce(_newJumpForce, ForceMode.Impulse);
        }
    }

    public bool IsPlayerMoving()
    {
        Rigidbody rb = clientController.playerRigidbody;
        if(rb.velocity.x != 0 || rb.velocity.y != 0 || rb.velocity.z != 0)
        {
            return true;
        }
        return false;
    }

    private void VelocityControl()
    {
        Vector3 currentVelocity = new Vector3(clientController.playerRigidbody.velocity.x, clientController.playerRigidbody.velocity.y, clientController.playerRigidbody.velocity.z);
        if(currentVelocity.magnitude > MovementSpeed)
        {
            Vector3 limitedVelocity = currentVelocity.normalized * MovementSpeed;
            clientController.playerRigidbody.velocity = new Vector3(limitedVelocity.x, limitedVelocity.y, limitedVelocity.z);
        }
    }

    public bool IsPlayerMovingSidewards()
    {
        if(clientController.mouse0 && clientController.rotationInput != 0)
        {
            return true;
        }
        return false;
    }
}
