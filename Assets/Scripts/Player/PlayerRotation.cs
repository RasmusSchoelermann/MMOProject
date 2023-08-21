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
        gameObject.transform.Rotate(0, (clientController._playerInput.x * clientController.rotationSpeed) * Time.deltaTime, 0);
        //clientController._playerInput.x
    }

}
