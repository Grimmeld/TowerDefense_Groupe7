using UnityEngine;

public class TargetRegister : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TargetManager.instance.RegisterEnemy(gameObject); //register l'ennemi en tant que target volant au start
    }

    private void OnDestroy()
    {
        TargetManager.instance.UnRegisterEnemy(gameObject); //si l'ennemi meurt, l'enlever de la liste d'ennemi disponible
    }
}
