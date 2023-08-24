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
        ApplyRotation();
    }

    public void ApplyRotation()
    {
        Vector3 inputDirection = clientController.orientation.forward * clientController.movementInput.y + clientController.orientation.right * clientController.rotationInput;
        if(inputDirection != Vector3.zero)
        {
            clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, inputDirection.normalized, Time.deltaTime * clientController.RotationSpeed);
        }
    }

    /*public void ApplyRotation()
    {
        if (clientController.playerTransform == null)
            return;

        clientController.playerTransform.Rotate(0, (clientController._rotationInput * clientController.RotationSpeed) * Time.deltaTime, 0);
    }

    public void RotatePlayerBy(float degrees)
    {
        if (clientController.playerTransform == null)
            return;

        if (clientController.playerTransform.rotation.y == degrees)
            return;

        Quaternion targetRotation = Quaternion.Euler(0, clientController.playerRigidbody.rotation.y + degrees, 0);

        clientController.playerRigidbody.rotation = targetRotation;
    }

    public void RotatePlayerInCameraDirection()
    {
        Vector3 x = Vector3.Cross(clientController.playerCam.transform.position.normalized, clientController.playerTransform.position.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta / Time.fixedDeltaTime;

        Quaternion q = clientController.playerTransform.rotation * clientController.playerRigidbody.inertiaTensorRotation;
        Vector3 T = q * Vector3.Scale(clientController.playerRigidbody.inertiaTensor, (Quaternion.Inverse(q) * w));

        if(clientController.playerTransform.rotation.eulerAngles.y != clientController.playerCam.transform.rotation.eulerAngles.y)
        {
            clientController.playerTransform.Rotate(0, T.y, 0);
        }
    }

    public bool IsPlayerOnlyRotating()
    {
        if (clientController._rotationInput != 0 && clientController._movementInput.y == 0)
        {
            return true;
        }
        return false;
    }
    */
}
