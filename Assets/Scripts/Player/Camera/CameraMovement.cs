using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraMovement : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerMovement playerMovement;
    private PlayerRotation playerRotation;

    private Vector2 previousMousePos = Vector2.zero;


    public void InitializeCamera(ClientAuthPlayerController controller, PlayerMovement pMovement, PlayerRotation protation)
    {
        clientController = controller;
        playerMovement = pMovement;
        playerRotation = protation;

        //_cameraOffset = transform.position - clientController.playerObject.position;
    }

    public void ApplyCameraRotation()
    {
        if(clientController.Mouse0Pressed() || clientController.Mouse0And1Pressed() && clientController.rotationInput == 0)
        {
            Vector2 mousePos = clientController.mousePosition;

            Vector2 input = clientController.mouseInput;
            input.x = input.x * clientController.MouseSensitivity;
            input.y = -input.y * clientController.MouseSensitivity;

            if(previousMousePos != mousePos)
            {
                clientController.cinemachineCameraSettings.m_XAxis.Value += input.x * 50f * Time.deltaTime;
                clientController.cinemachineCameraSettings.m_YAxis.Value += input.y * Time.deltaTime;
                previousMousePos = mousePos;
            }

        }
    }

    public void CharacterViewDirection()
    {
        Vector3 viewDirection = clientController.player.position - new Vector3(transform.position.x, clientController.player.position.y, transform.position.z);
        if (viewDirection != Vector3.zero)
            clientController.orientation.forward = viewDirection.normalized;
    }



}
