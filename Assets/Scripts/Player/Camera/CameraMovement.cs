using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerMovement playerMovement;
    private PlayerRotation playerRotation;

    [SerializeField]
    private Vector3 _cameraOriginPosition;

    private Vector3 _cameraRotation;
    private Vector3 _currentCameraPosition;

    [SerializeField]
    private float _cameraOffset = 10f;
    [SerializeField]
    private float _zoomSpeed = 1f;

    [SerializeField]
    private float _smoothSpeed = 0.125f;

    [SerializeField]
    private bool _manualCamMovement = false;

    private bool _rotateAroundPlayer = true;

    private float _rotationSpeed = 5.0f;


    public void InitializeCamera(ClientAuthPlayerController controller, PlayerMovement pMovement, PlayerRotation protation)
    {
        clientController = controller;
        playerMovement = pMovement;
        playerRotation = protation;

        //_cameraOffset = transform.position - clientController.playerObject.position;
    }

    public void FocusCamOnPlayer(Transform player)
    {
        transform.LookAt(player);
    }

    public void UpdateCamera()
    {
        if (!clientController.Mouse0)
            return;

        _cameraRotation.x += clientController.mouseInput.x * clientController.MouseSensitivity;
        _cameraRotation.y += clientController.mouseInput.y * clientController.MouseSensitivity;

        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -40, 40);


        transform.localEulerAngles = new Vector3(_cameraRotation.x, _cameraRotation.y);

        Vector3 startPos = new Vector3(clientController.player.position.x, clientController.player.position.y, clientController.player.position.z);
        transform.position = startPos - transform.forward * _cameraOffset;


    }

    public bool IsCameraAtOrigin()
    {
        if (transform.localPosition == _cameraOriginPosition)
        {
            return true;
        }
        return false;
    }

    /* public void ReturnCameraToOrigin()
     {
         Vector3 localCameraPosOnPlayer = clientController.playerTransform.TransformPoint(_cameraOriginPosition);
         Vector3 desiredPosition = localCameraPosOnPlayer - transform.forward * _cameraOffset;
         Vector3 smoothTransition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

         transform.position = smoothTransition;
         transform.LookAt(clientController.playerTransform);
     }

     public void PlayerCameraMovement()
     {
         if (!playerMovement.IsPlayerMoving())
             return;

         if (playerRotation.IsPlayerOnlyRotating())
             return;

         if (_manualCamMovement)
             return;

         Vector3 localCameraPosOnPlayer = clientController.playerTransform.TransformPoint(_cameraOriginPosition);
         Vector3 desiredPosition = localCameraPosOnPlayer;
         Vector3 smoothTransition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
         transform.position = smoothTransition;

         transform.LookAt(clientController.playerTransform);

         Vector3 localCameraPosOnPlayer = clientController.playerTransform.TransformPoint(_cameraOriginPosition);
         Vector3 desiredPosition = localCameraPosOnPlayer - transform.forward * _cameraOffset;
         Vector3 smoothTransition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

         transform.position = smoothTransition;
         transform.LookAt(clientController.playerTransform);
     }

     public void ManualCameraMovement()
     {
         if (!Input.GetMouseButton(1))
         {
             _manualCamMovement = false;
             _cameraRotation.x = 31;
             _cameraRotation.y = 0;
             return;
         }

         if (playerMovement.IsPlayerMovingSidewards())
             return;

         _manualCamMovement = true;
         _cameraRotation.y += -clientController._mouseInput.x;
         _cameraRotation.x += -clientController._mouseInput.y;

         _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -40, 40);


         transform.localEulerAngles = new Vector3(_cameraRotation.x, _cameraRotation.y);

         Vector3 startPos = new Vector3 (clientController.playerTransform.position.x, clientController.playerTransform.position.y + _cameraOriginPosition.y, clientController.playerTransform.position.z);
         transform.position = startPos - transform.forward * _cameraOffset;

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
         if(transform.position == clientController.playerTransform.TransformPoint(_cameraOriginPosition))
         {
             return true;
         }
         return false;
     }*/

}
