using UnityEngine;
using System;

public class PlayerRotation : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private CameraMovement cameraMovement;

    [SerializeField]
    private float rotationSpeed = 10;

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

    public void InitializePlayerRotation(ClientAuthPlayerController controller, CameraMovement camMove)
    {
        clientController = controller;
        cameraMovement = camMove;
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
            clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, inputDirection.normalized, Time.deltaTime * RotationSpeed);
        }
    }

    public void RotatePlayerOnPoint()
    {
        cameraMovement.cinemachineCameraSettings.m_XAxis.Value += clientController.rotationInput * 50f * Time.deltaTime;
        clientController.playerObject.forward = Vector3.Slerp(clientController.playerObject.forward, clientController.orientation.forward, Time.deltaTime * RotationSpeed);
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
