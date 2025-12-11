using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera Camera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] public bool MainCamera;
    [SerializeField] public bool MapCamera;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapCamera = true;
    }
    public void SwitchCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (MainCamera)
            {
                Camera.Priority = 0;
                MainCamera = false;
                MapCamera = true;
            }
            else if (MapCamera)
            {
                Camera.Priority = 20;
                MainCamera = true;
                MapCamera = false;
            }

        }
    }
}
