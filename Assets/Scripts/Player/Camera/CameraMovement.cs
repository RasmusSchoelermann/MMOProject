using UnityEngine;
using Cinemachine;


public class CameraMovement : MonoBehaviour
{
    private ClientAuthPlayerController clientController;

    private Vector2 previousMousePos = Vector2.zero;

    [SerializeField]
    private float[] orbitRadius;
    [SerializeField]
    private float[] orbitHeight;

    public CinemachineFreeLook cinemachineCameraSettings;

    [SerializeField]
    private float zoomAmount = 0f;
    [SerializeField]
    private float mouseSensitivity = 3.0f;

    public float ZoomAmount
    {
        get { return zoomAmount; }
        set
        {
            if (value <= 3 && value >= -3)
                zoomAmount = value;

        }
    }

    public float MouseSensitivity
    {
        get { return mouseSensitivity; }
        set { mouseSensitivity = value; }
    }


    public void InitializeCamera(ClientAuthPlayerController controller)
    {
        clientController = controller;
        EventManager.OnCamDampChanged.AddListener(SetCameraDamping);
    }

    public void ApplyCameraRotation()
    {
        if(clientController.Mouse0Pressed() || clientController.Mouse0And1Pressed() && clientController.rotationInput == 0)
        {
            Vector2 mousePos = clientController.mousePosition;

            Vector2 input = clientController.mouseInput;
            input.x = input.x * MouseSensitivity;
            input.y = -input.y * MouseSensitivity;

            if(previousMousePos != mousePos)
            {
                cinemachineCameraSettings.m_XAxis.Value += input.x * 50f * Time.deltaTime;
                cinemachineCameraSettings.m_YAxis.Value += input.y * Time.deltaTime;
                previousMousePos = mousePos;
            }

        }
    }



    public void ZoomCamera()
    {
        for(int i = 0; i < 3; i++) 
        {
            cinemachineCameraSettings.m_Orbits[i].m_Radius = orbitRadius[i] + -ZoomAmount;
            cinemachineCameraSettings.m_Orbits[i].m_Height = orbitHeight[i] + -ZoomAmount;
        }
    }

    public void CharacterViewDirection()
    {
        Vector3 viewDirection = clientController.playerObject.position - new Vector3(transform.position.x, clientController.player.position.y, transform.position.z);
        if (viewDirection != Vector3.zero)
            clientController.orientation.forward = viewDirection.normalized;
    }

    public void SetCameraDamping(float value)
    {
        for (int i = 0; i < 3; i++)
        {
            CinemachineOrbitalTransposer transposer = cinemachineCameraSettings.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>();
            transposer.m_XDamping = value;
        }
    }



}
