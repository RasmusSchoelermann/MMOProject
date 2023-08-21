using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerMovement playerMovement;

    [SerializeField]
    private Vector3 zoomOffset;

    [SerializeField]
    private float smoothSpeed = 0.125f;

    public void InitializeCamera(ClientAuthPlayerController controller, PlayerMovement pMovement)
    {
        clientController = controller;
        playerMovement = pMovement;
    }
    public void PlayerCameraMovement()
    {
        Vector3 localCameraPosOnPlayer = clientController.playerTransform.TransformPoint(new Vector3(0, 3, -5));

        if (!playerMovement.IsPlayerMoving() && !IsCameraBehindPlayer())
            return;

        if (clientController.playerRotation.IsPlayerOnlyRotating())
            return;

        Vector3 desiredPosition = localCameraPosOnPlayer + zoomOffset;
        Vector3 smoothTransition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothTransition;

        transform.LookAt(clientController.playerTransform);
    }

    public bool IsCameraBehindPlayer()
    {
        if(transform.position == clientController.playerTransform.TransformPoint(new Vector3(0, 3, -5)))
        {
            return true;
        }
        return false;
    }

}
