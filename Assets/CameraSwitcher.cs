using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera Camera;
    private bool MainCamera;
    private bool MapCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapCamera = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (MainCamera)
            {

                Camera.Priority = 0;
                MainCamera = false;
                //InputSystem.DisableDevice(Keyboard.current);
                MapCamera = true;
            }
            else if (MapCamera)
            {
                Camera.Priority = 20;
                MainCamera = true;
                //InputSystem.EnableDevice(Keyboard.current);
                MapCamera = false;
            }

        }
    }
}
