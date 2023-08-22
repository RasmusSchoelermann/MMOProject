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

    public void InitializePlayerMovement(ClientAuthPlayerController controller, PlayerRotation rotation, CameraMovement cam)
    {
        clientController = controller;
        playerRotation = rotation;
        camMovement = cam;
    }

    public void ApplyMovement()
    {
        float currentMovementSpeed = clientController._movementSpeed;
        if (clientController.IsGrounded())
        {
        
            clientController._newVelocity.Set(clientController.playerRigidbody.velocity.x, 0.0f, clientController._movementInput.y * (currentMovementSpeed * 100) * Time.deltaTime);
            clientController.playerRigidbody.AddRelativeForce(new Vector3(0, 0, clientController._newVelocity.z));


        }
        else if(!clientController.IsGrounded())
        {
            clientController._newVelocity.Set(clientController.playerRigidbody.velocity.x, clientController.playerRigidbody.velocity.y, clientController._movementInput.y * (currentMovementSpeed * 100) * Time.deltaTime);
            clientController.playerRigidbody.AddRelativeForce(new Vector3(clientController._newVelocity.x, clientController._newVelocity.y, clientController._newVelocity.z));
        }

    }

    public void ApplyJump()
    {
        if (clientController.IsGrounded())
        {
            clientController._newForce.Set(0.0f, clientController._jumpForce, 0.0f);
            clientController.playerRigidbody.AddForce(clientController._newForce, ForceMode.Impulse);
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
}
