using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<SubWave> Subwaves;
    }
    [System.Serializable]
    public class SubWave
    {
        public List<EnemyType> EnemyTypes;
        //public int count;
        public float rate;
        public Transform spawnPoints; // spawn
    }
    [System.Serializable]
    public class EnemyType
    {
        public GameObject[] enemies;
        public int count;
    }
    [Header("Liste des Vagues et sous vague")]
    private List<EnemyType> enemies;
    private List<EnemyType> count;
    public UnityEvent onAllWavesCompleted;
    public Wave[] waves;
    private SubWave[] SousVague;
    private EnemyType[] Enemy;

    private int currentWave = -1;
    private int currentSubWave = -1;
    //private int EnemyTypeCount = 2;

    [Header("Condition pour la prochaine sous-vague")]
    public int EnnemiesAlive;
    public float TimeBetweenSubWaves = 15f;


    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        TimeBetweenSubWaves = 15f;
    }

    private void Update()
    {

        TimeBetweenSubWaves -=Time.deltaTime;
        if (EnnemiesAlive > 0)
            return;
        NextWave();

    }

    void NextWave()
    {


        if (currentWave >= waves.Length) //toute les vague terminé
        {
            Debug.Log(" vagues terminées");
            onAllWavesCompleted.Invoke();
            return;
        }

        if (TimeBetweenSubWaves <= 0 || EnnemiesAlive <=0)
        {
            currentWave++;
            NextSubWave();
            Debug.Log("Etape1");
        }


    }

    void NextSubWave()
    {
        TimeBetweenSubWaves = 15;
        currentSubWave++;
        Wave wave = waves[currentWave];
        Debug.Log("Etape2");
        if (currentSubWave >= wave.Subwaves.Count)
        {
            NextWave();
            return;
        }
        if (EnnemiesAlive >0)
            return;
        StartCoroutine(SpawnSubWave(wave.Subwaves[currentSubWave]));

    }

    IEnumerator SpawnSubWave(SubWave sub)
    {
        Debug.Log("Spawn vague ");// + (currentWave + 1));
        Wave wave = waves[currentWave];
        foreach(SubWave subWaves in wave.Subwaves)
        {
            foreach (EnemyType type in subWaves.EnemyTypes)
            {
                for (int i = 0; i < type.count; i++)
                {
                    Debug.Log("Etape3");
                    GameObject enemy = type.enemies[0];
                    Transform spawnPoint = subWaves.spawnPoints;
                    SpawnEnemy(enemy, spawnPoint);
                    yield return new WaitForSeconds(1f / subWaves.rate);
                }
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform Spawn)
    {
        Debug.Log("Etape4");
        Instantiate (enemyPrefab, Spawn.position, Spawn.rotation);
    }




    public void OnEnemyDied(GameObject enemy)
    {
        if (EnnemiesAlive <= 0 || TimeBetweenSubWaves < 0)
            NextSubWave();
    }
}