using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class CameraZoo : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector2 inputMove;
    [SerializeField] private float speedMove;

    private void Update()
    {
        float speed = Time.deltaTime * speedMove;
        transform.Translate(speed * inputMove.x, 0, speed * inputMove.y, Space.World);
    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

}
