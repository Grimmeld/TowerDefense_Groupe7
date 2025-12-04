using UnityEngine;

public class CanvaManagement : MonoBehaviour
{
    private Camera cameraScene;
    private Canvas canvas;
    [SerializeField]private float _scaleFactor = 0.1f;
    private void Start()
    {
        cameraScene = Camera.main;
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (cameraScene != null && canvas != null)
        {
            canvas.transform.rotation = cameraScene.transform.rotation;

            float distanceToCamera = Vector3.Distance(cameraScene.transform.position, transform.position);
            float camHeight = 2.0f * distanceToCamera * Mathf.Tan(Mathf.Deg2Rad * (cameraScene.fieldOfView * 0.5f));

            float scale = camHeight * _scaleFactor;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
