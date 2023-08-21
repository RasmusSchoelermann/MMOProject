using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private ClientAuthPlayerController _clientController;
    private PlayerRotation _playerRotation;

    public void InitializePlayerMovement(ClientAuthPlayerController controller, PlayerRotation rotation)
    {
        _clientController = controller;
        _playerRotation = rotation;
    }

    public void ApplyMovement()
    {
        float currentMovementSpeed = _clientController.movementSpeed;
        if (_clientController.IsGrounded())
        {
            if(_clientController._playerInput.y < 0)
            {
                currentMovementSpeed /= 2;
            }

            _clientController.newVelocity.Set(_clientController.playerRigidbody.velocity.x, 0.0f, _clientController._playerInput.y * (currentMovementSpeed * 100) * Time.deltaTime);
            //_clientController.playerRigidbody.velocity = _clientController.newVelocity;
            _clientController.playerRigidbody.AddRelativeForce(new Vector3(0, 0, _clientController.newVelocity.z));


        }
        else if(!_clientController.IsGrounded())
        {
            _clientController.newVelocity.Set(_clientController.playerRigidbody.velocity.x, _clientController.playerRigidbody.velocity.y, _clientController._playerInput.y * (currentMovementSpeed * 100) * Time.deltaTime);
            //_clientController.playerRigidbody.velocity = _clientController.newVelocity;
            _clientController.playerRigidbody.AddRelativeForce(new Vector3(_clientController.newVelocity.x, _clientController.newVelocity.y, _clientController.newVelocity.z));
        }

    }

    public void ApplyJump()
    {
        if (_clientController.IsGrounded())
        {
            _clientController.newForce.Set(0.0f, _clientController.jumpForce, 0.0f);
            _clientController.playerRigidbody.AddForce(_clientController.newForce, ForceMode.Impulse);
        }
    }
}
