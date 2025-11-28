using NUnit;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

public class Enemy_Sapper : MonoBehaviour
{
    [SerializeField] public Transform targetPosition;
    public string TurretTag = "Turret";
    public Vector3 enemyPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        FindTarget();
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = transform.position;
        Vector3 NewPos = new Vector3(enemyPos.x, 4, enemyPos.z);
        if (Vector3.Distance(transform.position, targetPosition.position) < 4f)
        {
            NewPos = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z);
            transform.LookAt(targetPosition.position);
        }
        if (targetPosition == null)
            FindTarget();
        Vector3 direction = Vector3.MoveTowards(NewPos, targetPosition.position, 1 * Time.deltaTime);
        
        transform.position = direction;


    }


    public void FindTarget()
    {
        
        GameObject[] target = GameObject.FindGameObjectsWithTag(TurretTag);
        GameObject destination = target[Random.Range(0, target.Length)];
        targetPosition = destination.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(TurretTag))
        {
            BaseTower TowerScript = other.GetComponent<BaseTower>();
            TowerScript.Sabotaged = true;
            Destroy(gameObject);
        }
    }
}
