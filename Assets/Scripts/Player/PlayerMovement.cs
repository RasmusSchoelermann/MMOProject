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

    public void InitializePlayerMovement(ClientAuthPlayerController controller, PlayerRotation rotation, CameraMovement cam)
    {
        clientController = controller;
        playerRotation = rotation;
        camMovement = cam;
    }

    public void CheckPlayerMovement()
    {
        _currentMovementSpeed = clientController.MovementSpeed;
        if(!clientController.IsGrounded())
        {
            FallingControls();
            return;
        }
        ApplyMovement();

    }

    private void ApplyMovement()
    {


        /*float currentMovementSpeed = clientController.MovementSpeed;
        if (clientController.IsGrounded())
        {
            float xVelocity = (clientController._movementInput.x * (currentMovementSpeed * 100) * Time.deltaTime);
            float zVelocity = (clientController._movementInput.y * (currentMovementSpeed * 100) * Time.deltaTime);
            if (Input.GetMouseButton(1) && clientController._movementInput.x != 0)
            {
                ApplySidewardsMovement(xVelocity, zVelocity);
            }
            else
            {
                _newVelocity.Set(clientController.playerRigidbody.velocity.x, 0.0f, zVelocity);
            }
            
            clientController.playerRigidbody.AddRelativeForce(new Vector3(_newVelocity.x, 0, _newVelocity.z));


        }
        else if(!clientController.IsGrounded())
        {
            _newVelocity.Set(clientController._movementInput.x * (currentMovementSpeed * 100) * Time.deltaTime, clientController.playerRigidbody.velocity.y, clientController._movementInput.y * (currentMovementSpeed * 100) * Time.deltaTime);
            clientController.playerRigidbody.AddRelativeForce(new Vector3(_newVelocity.x, _newVelocity.y, _newVelocity.z));
        }*/

    }

    private void ApplySidewardsMovement(float xVelocity, float zVelocity)
    {
        /*_newVelocity.Set(xVelocity, 0.0f, zVelocity);

        if (zVelocity == 0)
        {
            if (clientController._movementInput.x < 0)
            {
                playerRotation.RotatePlayerBy(-90);
            }
            else
            {
                playerRotation.RotatePlayerBy(90);
            }
        }
        else
        {
            if (clientController._movementInput.x < 0 && clientController._movementInput.y != 0)
            {
                playerRotation.RotatePlayerBy(-45);
            }
            else if(clientController._movementInput.x > 0 && clientController._movementInput.y != 0)
            {
                playerRotation.RotatePlayerBy(45);
            }
        }*/
    }

    private void FallingControls()
    {
        _newVelocity.Set(clientController.movementInput.x * _currentMovementSpeed, clientController.playerRigidbody.velocity.y, clientController.movementInput.y * _currentMovementSpeed);
        clientController.playerRigidbody.AddRelativeForce(new Vector3(_newVelocity.x, _newVelocity.y, _newVelocity.z));
    }

    public void ApplyJump()
    {
        if (clientController.IsGrounded())
        {
            _newJumpForce.Set(0.0f, clientController.MovementSpeed, 0.0f);
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

    public bool IsPlayerMovingSidewards()
    {
        if(Input.GetMouseButton(1) && clientController.movementInput.x != 0)
        {
            return true;
        }
        return false;
    }
}
