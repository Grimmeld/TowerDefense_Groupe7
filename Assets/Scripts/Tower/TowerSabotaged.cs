using UnityEngine;
using System.Collections;

public class TowerSabotaged : MonoBehaviour
{
    public bool Sabotaged;
    public float SapDuration;

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
}
