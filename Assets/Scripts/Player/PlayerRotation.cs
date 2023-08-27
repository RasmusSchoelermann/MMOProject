using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private ClientAuthPlayerController clientController;

    public void InitializePlayerRotation(ClientAuthPlayerController controller)
    {
        clientController = controller;
    }

    public void CheckRotationInput()
    {
        if (IsPlayerOnlyRotating())
        {
            RotatePlayerOnPoint();
        }
        else
        {
            ApplyRotation();
        }
    }

    public void ApplyRotation()
    {

        Vector3 inputDirection = clientController.orientation.forward * clientController.movementInput.y + clientController.orientation.right * clientController.movementInput.x;
        if(inputDirection != Vector3.zero)
        {
            clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, inputDirection.normalized, Time.deltaTime * clientController.RotationSpeed);
        }
    }

    public void RotatePlayerOnPoint()
    {
        clientController.cinemachineCameraSettings.m_XAxis.Value += clientController.rotationInput * 50f * Time.deltaTime;
        clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, clientController.orientation.forward, Time.deltaTime * clientController.RotationSpeed);
    }
   
    public bool IsPlayerOnlyRotating()
    {
        if (clientController.rotationInput != 0 && clientController.movementInput == Vector2.zero && !clientController.Mouse0Pressed())
        {
            return true;
        }
        return false;
    }
}
