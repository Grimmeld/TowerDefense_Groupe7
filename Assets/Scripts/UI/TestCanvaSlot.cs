using System.Data;
using UnityEngine;

public class TestCanvaSlot : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Vector3 target;

    private void Update()
    {
        if (target != null)
        {
            //panel.transform.position = Camera.main.WorldToScreenPoint(target);

            var position = Camera.main.WorldToScreenPoint(target);
            position.z = (panel.transform.position - Camera.main.transform.position).magnitude;
            panel.transform.position = Camera.main.ScreenToWorldPoint(position);


        }
    }
}
