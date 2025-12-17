using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    private void Start()
    {
        if (instance != null)
            return;

        instance = this;
    }

    public void Defeat()
    {
        Debug.Log("end of game");
        

    }


}
