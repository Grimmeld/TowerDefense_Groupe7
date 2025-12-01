using UnityEngine;
using System.Collections;

public class Shrapnel : MonoBehaviour
{
    [Header("Config de la courbe")]
    public float duration = 1f;
    public float height = 1f;

    private Vector3 StartPos;
    public Vector3 TargetPos;
    public Vector3 Offset;
    private float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPos = transform.localPosition;
        Vector3 offSet = transform.localPosition + Offset;
        TargetPos = offSet;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        Vector3 pos = Vector3.Lerp(StartPos, TargetPos, t);
        pos.y += height * Mathf.Sin(Mathf.PI * t);
        transform.position = pos;

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}

