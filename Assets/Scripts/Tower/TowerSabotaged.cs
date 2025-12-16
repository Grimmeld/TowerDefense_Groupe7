using UnityEngine;
using System.Collections;

public class TowerSabotaged : MonoBehaviour
{
    Tower_Animation tower_Animation;
    public bool Sabotaged;
    public float SapDuration;

    private void Awake()
    {
        tower_Animation = GetComponent<Tower_Animation>();
    }

    public void Sapping()
    {
        StartCoroutine(Sapped());
    }
    IEnumerator Sapped()
    {
        Sabotaged = true;
        yield return new WaitForSeconds(SapDuration);
        Sabotaged = false;
    }

    private void Update()
    {
        if (Sabotaged)
        {
            tower_Animation.Sapped();
        }
    }
}
