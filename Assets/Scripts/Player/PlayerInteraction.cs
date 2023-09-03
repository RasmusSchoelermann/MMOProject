using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private ClientAuthPlayerController clientController;
    private PlayerCombat playerCombat;

    public void InitializePlayerInteraction(ClientAuthPlayerController controller, PlayerCombat pCombat)
    {
        clientController = controller;
        EventManager.OnMouseClickRayCast.AddListener(RaycastOfMousePos);
    }

    private void RaycastOfMousePos(Vector2 mousePos)
    {
        Ray ray = clientController.playerCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            playerCombat.Target = hit.transform.gameObject;
        }

        if (playerCombat.Target != null)
            SelectTarget();
    }

    private void SelectTarget()
    {
        //UI etc.
    }
}
