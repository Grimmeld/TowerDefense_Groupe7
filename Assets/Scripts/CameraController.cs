using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // Manage the movement of the camera
    // - Move on the axe X & Z  
    // - Rotation on axe yaw 

    // Movement is manage from an empty object -> Rotation from this point

    // Manage the distance of the camera from the manager

    CameraSwitcher cameraSwitcher;
    void Awake()
    {
        cameraSwitcher = GetComponent<CameraSwitcher>();
    }

    [SerializeField] private Vector3 cameraOffset;

    [Header("Movement")]
    [SerializeField] private Vector2 inputMove;
    [SerializeField] private float speedMove;


    [Header("Rotation")]
    [SerializeField] private float speedRotation;
    private bool turnLeft;
    private bool turnRight;
    private float rotationY;
    [SerializeField] private float rangeDistance; 

    private void Start()
    {
        SetOffsetCamera();
    }


    private void Update()
    {
        if (cameraSwitcher.MapCamera)
        {
            speedMove = 20f;
        }
        else if (cameraSwitcher.MainCamera)
        {
            speedMove = 0f;
        }
        if (inputMove != Vector2.zero)
        {
            if (transform.position.x <= 33f && transform.position.x >= -33f && transform.position.z <= 35f && transform.position.z >= -35f)
            {
                float speed = Time.deltaTime * speedMove;

                transform.Translate(speed * inputMove.x, 0, speed * inputMove.y);
            }
            else if (transform.position.x > 33f && inputMove.x < 0)
            {
                float speed = Time.deltaTime * speedMove;
                transform.Translate(speed * inputMove.x, 0, speed * inputMove.y);
            }
            else if (transform.position.x < -33f && inputMove.x > 0)
            {
                float speed = Time.deltaTime * speedMove;
                transform.Translate(speed * inputMove.x, 0, speed * inputMove.y);
            }
            else if (transform.position.z > 35f && inputMove.y < 0)
            {
                float speed = Time.deltaTime * speedMove;
                transform.Translate(speed * inputMove.x, 0, speed * inputMove.y);
            }
            else if (transform.position.z < -35f && inputMove.y > 0)
            {
                float speed = Time.deltaTime * speedMove;
                transform.Translate(speed * inputMove.x, 0, speed * inputMove.y);
            }

        }

        if (turnLeft || turnRight)
        {
            Vector3 currentAngles = new Vector3(
                0,
                Mathf.LerpAngle(transform.eulerAngles.y, rotationY, Time.deltaTime * speedRotation),
                0);


            transform.eulerAngles = currentAngles;


            if (turnLeft)
            {
                if (transform.eulerAngles.y - rotationY < rangeDistance)
                {
                    transform.eulerAngles = new Vector3(0, rotationY, 0);
                    turnLeft = false;
                }
            }
            else if (turnRight)
            {
                if (rotationY - transform.eulerAngles.y < rangeDistance)
                {
                    transform.eulerAngles = new Vector3(0, rotationY, 0);
                    turnRight = false;
                }
            }
        }



    }


    // METHODE

    private void SetOffsetCamera()
    {
        Camera.main.transform.position = cameraOffset;
    }

    // INPUT ACTION

    public void MoveCamera(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    public void RotationLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!turnLeft && !turnRight)
            {
                turnLeft = true;

                if (transform.rotation.eulerAngles.y - 90f < 0)
                {
                    rotationY = 270f;
                }
                else
                {
                    rotationY = transform.rotation.eulerAngles.y - 90f;
                }

            }
        }
    }

    public void RotationRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!turnRight && !turnLeft)
            {
                turnRight = true;

                if (transform.rotation.eulerAngles.y + 90f >= 360f)
                {
                    rotationY = 360f;
                }
                else
                {
                    rotationY = transform.rotation.eulerAngles.y + 90f;
                }

            }
        }
    }

    public void MouseScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        if(transform.position.y <= 10f && transform.position.y >= 2f)
        {
            transform.Translate(0, -scrollValue, 0);
        }
        else if(transform.position.y > 10f && scrollValue > 0)
            transform.Translate(0, -scrollValue, 0);
        else if(transform.position.y < 2f && scrollValue < 0)
        transform.Translate(0, -scrollValue, 0);
    }

}
