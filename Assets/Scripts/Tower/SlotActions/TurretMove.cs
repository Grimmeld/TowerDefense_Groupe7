using UnityEngine;

public class TurretMove : MonoBehaviour
{

    public static TurretMove instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public bool isMoving;

    [SerializeField] private GameObject selectedTower;
    public Transform targetSlot;
    public void Move(GameObject tower)
    {
        Debug.Log("TurretMove Move called");
        Debug.Log("Move : " + tower.name);

        EnableIsMoving(true);
        selectedTower = tower;

    }

    public void EnableIsMoving(bool enable)
    {
        isMoving = enable;
    }

    public GameObject GetTower()
    {
         return selectedTower;
    }

    public void DeleteTower()
    {
        Destroy(selectedTower);
    }

    public GameObject InstantiateMovedTower(GameObject turret, Transform SpawnPoint)
    {
        TurretSlot turretSlot = GetComponent<TurretSlot>();

        GameObject go = (GameObject)Instantiate(selectedTower, turretSlot.transform);
        go.transform.position = SpawnPoint.position; // Positionner la tour sur le slot
        turretSlot.UpdateTurretSlot(go); // Update tour dans les paramètres du slot
        EnableIsMoving(false);
        Zone zone = turretSlot.GetComponentInParent<Zone>();

        if (zone != null)
        {
            TowerController towerController = go.GetComponent<TowerController>();
            if (towerController != null)
            {
                towerController.enabled = zone.isPowered;
            }
        }

        DeleteTower();

        return go;


    }
}
