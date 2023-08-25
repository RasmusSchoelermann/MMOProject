using Steamworks;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerRotation playerRotation;
    private CameraMovement camMovement;

    private Vector3 _newVelocity;
    private Vector3 _newJumpForce;

    private float _currentMovementSpeed;

    private Vector3 _moveDirection;

    [SerializeField]
    private float _groundDrag;

    public void InitializePlayerMovement(ClientAuthPlayerController controller, PlayerRotation rotation, CameraMovement cam)
    {
        clientController = controller;
        playerRotation = rotation;
        camMovement = cam;
    }

    public void CheckPlayerMovement()
    {
        _currentMovementSpeed = clientController.MovementSpeed;
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

        if (clientController.Mouse0Pressed() && clientController.rotationInput != 0)
            ApplySidewardsMovementAD();

        if (clientController.movementInput != Vector2.zero)
            ApplyMovement();


    }

    private void ApplyMovement()
    {
        if (clientController.movementInput.x == 0)
        {
            clientController.SetCameraDamping(1f);
        }
        else
        {
            clientController.SetCameraDamping(0f);
        }

        _moveDirection = clientController.orientation.forward * clientController.movementInput.y + clientController.orientation.right * clientController.movementInput.x;

        clientController.playerRigidbody.AddForce(_moveDirection.normalized * _currentMovementSpeed * 10f, ForceMode.Force);

        if( clientController.rotationInput != 0)
        {
            playerRotation.RotatePlayerOnPoint();
        }

    }

    public void ApplySidewardsMovementAD()
    {
        clientController.SetCameraDamping(0f);
        _moveDirection = clientController.orientation.forward * clientController.movementInput.y + clientController.orientation.right * clientController.rotationInput;

        clientController.playerRigidbody.AddForce(_moveDirection.normalized * _currentMovementSpeed * 10f, ForceMode.Force);
        //clientController.playerObject.forward = Vector3.Slerp(clientController.orientation.forward, clientController.orientation.right * clientController.rotationInput, Time.deltaTime * clientController.RotationSpeed);
    }

    public void ApplyForwardsMovementWithMouse()
    {
        clientController.SetCameraDamping(0f);
        _moveDirection = clientController.orientation.forward * 1.0f;

        clientController.playerRigidbody.AddForce(_moveDirection.normalized * _currentMovementSpeed * 10f, ForceMode.Force);
        clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, clientController.orientation.forward, Time.deltaTime * clientController.RotationSpeed);
    }


    private void FallingControls()
    {
        _newVelocity.Set(clientController.movementInput.x * _currentMovementSpeed / 2, clientController.playerRigidbody.velocity.y, clientController.movementInput.y * _currentMovementSpeed / 2);
        clientController.playerRigidbody.AddRelativeForce(new Vector3(_newVelocity.x, _newVelocity.y, _newVelocity.z));
    }

    public void ApplyJump()
    {
        if (clientController.IsGrounded())
        {
            _newJumpForce.Set(0.0f, clientController.JumpForce, 0.0f);
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
        if(currentVelocity.magnitude > clientController.MovementSpeed)
        {
            Vector3 limitedVelocity = currentVelocity.normalized * clientController.MovementSpeed;
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
