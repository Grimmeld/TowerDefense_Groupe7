using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;
    EventManager eventManager;
    public void Awake()
    {
        Instance = this;
        eventManager = GetComponent<EventManager>();
    }
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
    public UnityEvent onBuildModeStart;
    public Wave[] waves;
    private SubWave[] SousVague;
    private EnemyType[] Enemy;

    public int currentWave = -1;
    private int currentSubWave = -1;
    //private int EnemyTypeCount = 2;

    [Header("Condition pour la prochaine sous-vague")]
    public int EnnemiesAlive;
    public float TimeBetweenSubWaves = 15f;
    public bool buildMode = false;
    private bool test = false;

    [Header("UI")]
    public GameObject NextWaveButton;

    private List<GameObject> aliveEnemies = new List<GameObject>();



    private void Start()
    {

        TimeBetweenSubWaves = 15f;
        NextWave();

        if(TimeBetweenSubWaves <=0)
        {
            NextSubWave();
        }
    }

    private void Update()
    {
        if (currentWave >= waves.Length) //toute les vague terminé
        {
            Debug.Log(" vagues terminées");
            onAllWavesCompleted.Invoke();
            NextWaveButton.SetActive(false);
            return;
        }
        if (buildMode)
            return;
        if (test)
            return;
        TimeBetweenSubWaves -= Time.deltaTime;
        if (TimeBetweenSubWaves <= 0 || EnnemiesAlive <= 0)
            NextSubWave();
    }

    void NextWave()
    {



        currentWave++;    
        currentSubWave = -1;
        NextSubWave();

    }

    void NextSubWave()
    {
        TimeBetweenSubWaves = 15;
        currentSubWave++;
        Wave wave = waves[currentWave];
        if (currentSubWave >= wave.Subwaves.Count)
        {
            //startBuildMode();
            return;
        }
        StartCoroutine(SpawnSubWave(wave.Subwaves[currentSubWave]));

    }

    IEnumerator SpawnSubWave(SubWave sub)
    {
        foreach (EnemyType type in sub.EnemyTypes)
        {
            for (int i = 0; i < type.count; i++)
            {
                GameObject enemyPrefab = type.enemies[0];
                SpawnEnemy(enemyPrefab, sub.spawnPoints);
                yield return new WaitForSeconds(1f / sub.rate);
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform Spawn)
    {
        Instantiate (enemyPrefab, Spawn.position, Spawn.rotation);
    }

    void startBuildMode()
    {
        if(NextWaveButton != null) 
            
        NextWaveButton.SetActive(true);
        ResourceManager.instance.AddGold(EventManager.Instance.GoldBonus);
        if (buildMode)
            return;
        buildMode = true;
    }


    public void OnEnemyDied()
    {
        EnnemiesAlive--;

        if(EnnemiesAlive >0)
        {
            return;
        }

        bool lastSubWave = currentSubWave +1 >= waves[currentWave].Subwaves.Count;

        if(lastSubWave && EnnemiesAlive <= 0)
        {
            startBuildMode();
            test = true;
        }
        else
        {
            //NextSubWave();
        }
    }

    public void StartNextWave()
    {
        TimeBetweenSubWaves = 15f;
        currentSubWave = -1;
        buildMode = false;
        NextWaveButton.SetActive(false);
        

        NextWave();
        StartCoroutine(Waiting());
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds (1);
        test = false;
    }

    public void ChangeState()
    {
        
        GameManager.Instance.ChangeState(GameManager.GameSate.CardSelection);
    }
}