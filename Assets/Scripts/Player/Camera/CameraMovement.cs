using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerMovement playerMovement;
    private PlayerRotation playerRotation;

    private Vector2 _cameraRotation;


    [SerializeField]
    private float _cameraOffset = 5.0f;
    [SerializeField]
    private float _zoomSpeed = 1f;

    [SerializeField]
    private float _smoothSpeed = 0.125f;

    private bool _manualCamMovement = false;

    public void InitializeCamera(ClientAuthPlayerController controller, PlayerMovement pMovement, PlayerRotation protation)
    {
        clientController = controller;
        playerMovement = pMovement;
        playerRotation = protation;
    }

    public void ReturnCameraToOrigin()
    { 
    }

    public void PlayerCameraMovement()
    {
        if (!playerMovement.IsPlayerMoving() && !IsCameraBehindPlayer())
            return;

        if (playerRotation.IsPlayerOnlyRotating())
            return;

        if (_manualCamMovement)
            return;

        Vector3 localCameraPosOnPlayer = clientController.playerTransform.TransformPoint(new Vector3(0, 3, -_cameraOffset));
        Vector3 desiredPosition = localCameraPosOnPlayer;
        Vector3 smoothTransition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothTransition;

        transform.LookAt(clientController.playerTransform);
    }

    public void ManualCameraMovement()
    {
        if (!Input.GetMouseButton(1))
        {
            _manualCamMovement = false;
            return;
        }

        if (playerMovement.IsPlayerMovingSidewards())
            return;

        _manualCamMovement = true;
        _cameraRotation.y += -clientController._mouseInput.x;
        _cameraRotation.x += -clientController._mouseInput.y;

        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -40, 40);


        transform.localEulerAngles = new Vector3(_cameraRotation.x, _cameraRotation.y);
        transform.position = clientController.playerTransform.position - transform.forward * _cameraOffset;

    }

    public void ZoomCamera()
    {
        if(clientController.playerCam.orthographic)
        {
            clientController.playerCam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        }
        else
        {
            clientController.playerCam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        }
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
