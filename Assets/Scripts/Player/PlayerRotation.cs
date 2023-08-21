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

    public void ApplyRotation()
    {
        if (clientController.playerTransform == null)
            return;

        clientController.playerTransform.Rotate(0, (clientController._playerInput.x * clientController._rotationSpeed) * Time.deltaTime, 0);
    }

    public bool IsPlayerOnlyRotating()
    {
        if (clientController._playerInput.x != 0 && clientController._playerInput.y == 0)
        {
            return true;
        }
        return false;
    }

}
