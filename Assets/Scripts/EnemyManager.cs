using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [SerializeField] private Transform mercury;
    [SerializeField] private float mercuryMin = 10;
    [SerializeField] private float mercuryMax = 15;
    [SerializeField] private Transform venus;
    [SerializeField] private float venusMin = 10;
    [SerializeField] private float venusMax = 15;
    [SerializeField] private Transform earth;
    [SerializeField] private float earthMin = 10;
    [SerializeField] private float earthMax = 15;
    [SerializeField] private Transform mars;
    [SerializeField] private float marsMin = 10;
    [SerializeField] private float marsMax = 15;
    [SerializeField] private Transform jupiter;
    [SerializeField] private float jupiterMin = 10;
    [SerializeField] private float jupiterMax = 15;
    [SerializeField] private Transform saturn;
    [SerializeField] private float saturnMin = 10;
    [SerializeField] private float saturnMax = 15;
    [SerializeField] private Transform uranus;
    [SerializeField] private float uranusMin = 10;
    [SerializeField] private float uranusMax = 15;
    [SerializeField] private Transform neptune;
    [SerializeField] private float neptuneMin = 10;
    [SerializeField] private float neptuneMax = 15;
    
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialEnemyCount = 5;
    [SerializeField] private int maxEnemyCount = 23;
    [SerializeField] private int currentEnemyCount = 0;
    [SerializeField] private float spawnIntervalMin = 3f;
    [SerializeField] private float spawnIntervalMax = 5f;
    [SerializeField] private int enemiesOnMercury = 0;
    [SerializeField] private int enemiesOnVenus = 0;
    [SerializeField] private int enemiesOnEarth = 0;
    [SerializeField] private int enemiesOnMars = 0;
    [SerializeField] private int enemiesOnJupiter = 0;
    [SerializeField] private int enemiesOnSaturn = 0;
    [SerializeField] private int enemiesOnUranus = 0;
    [SerializeField] private int enemiesOnNeptune = 0;
    
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private float corruptionRate = 0.1f;

    private float _corruption;
    private Transform[] _planets = new Transform[8];

    public void EnemyDestroyed(PlanetType targetTag)
    {
        switch (targetTag)
        {
            case PlanetType.Mercury:
                enemiesOnMercury--;
                break;
            case PlanetType.Venus:
                enemiesOnVenus--;
                break;
            case PlanetType.Earth:
                enemiesOnEarth--;
                break;
            case PlanetType.Mars:
                enemiesOnMars--;
                break;
            case PlanetType.Jupiter:
                enemiesOnJupiter--;
                break;
            case PlanetType.Saturn:
                enemiesOnSaturn--;
                break;
            case PlanetType.Uranus:
                enemiesOnUranus--;
                break;
            case PlanetType.Neptune:
                enemiesOnNeptune--;
                break;
        }
        
        currentEnemyCount--;
        _corruption -= 0.1f;
        hudManager.UpdateEnemyCounts(currentEnemyCount, enemiesOnMercury, enemiesOnVenus,
            enemiesOnEarth, enemiesOnMars, enemiesOnJupiter, enemiesOnSaturn, enemiesOnUranus, enemiesOnNeptune);
    }

    private void Start()
    {
        Instance = this;
        
        _planets = new[]
        {
            mercury, venus, earth, mars, jupiter, saturn, uranus, neptune
        };
        
        enemiesOnMercury = 0;
        enemiesOnVenus = 0;
        enemiesOnEarth = 0;
        enemiesOnMars = 0;
        enemiesOnJupiter = 0;
        enemiesOnSaturn = 0;
        enemiesOnUranus = 0;
        enemiesOnNeptune = 0;
        
        StartCoroutine(SpawnEnemies());
        
        Time.timeScale = 1;
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnRandomEnemy();
        }
        hudManager.UpdateEnemyCounts(currentEnemyCount, enemiesOnMercury, enemiesOnVenus,
            enemiesOnEarth, enemiesOnMars, enemiesOnJupiter, enemiesOnSaturn, enemiesOnUranus, enemiesOnNeptune);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
            
            if (currentEnemyCount < maxEnemyCount)
            {
                SpawnRandomEnemy();
                hudManager.UpdateEnemyCounts(currentEnemyCount, enemiesOnMercury, enemiesOnVenus,
                    enemiesOnEarth, enemiesOnMars, enemiesOnJupiter, enemiesOnSaturn, enemiesOnUranus, enemiesOnNeptune);
            }
        }
    }
    
    private void Update()
    {
        _corruption += currentEnemyCount * Time.deltaTime * corruptionRate;
        if (_corruption >= 1f)
        {
            hudManager.GameOver("Root Corruption");
            Destroy(this);
        }
        _corruption = Mathf.Clamp(_corruption, 0f, 1f);
        hudManager.SetCorruptionDisplay(_corruption);
        
        hudManager.UpdatePlanetTrackers(new []{enemiesOnMercury, enemiesOnVenus, enemiesOnEarth,
            enemiesOnMars, enemiesOnJupiter, enemiesOnSaturn, enemiesOnUranus, enemiesOnNeptune}, _planets);
    }

    private void SpawnRandomEnemy()
    {
        currentEnemyCount++;
        var planet = (PlanetType) Random.Range(0, Enum.GetNames(typeof(PlanetType)).Length);
        var enemyType = (EnemyType) Random.Range(0, Enum.GetNames(typeof(EnemyType)).Length);
        SpawnEnemy(planet, enemyType);
    }

    private void SpawnEnemy(PlanetType planet, EnemyType type)
    {
        var position = Vector3.zero;
        Transform targetPlanetTransform = null;
        
        switch (planet)
        {
            case PlanetType.Mercury:
                Vector3 randomDirection = Random.onUnitSphere;
                float randomDistance = Random.Range(mercuryMin, mercuryMax);
                position = mercury.position + randomDirection * randomDistance;
                targetPlanetTransform = mercury;
                enemiesOnMercury++;
                break;
            
            case PlanetType.Venus:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(venusMin, venusMax);
                position = venus.position + randomDirection * randomDistance;
                targetPlanetTransform = venus;
                enemiesOnVenus++;
                break;
            
            case PlanetType.Earth:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(earthMin, earthMax);
                position = earth.position + randomDirection * randomDistance;
                targetPlanetTransform = earth;
                enemiesOnEarth++;
                break;
            
            case PlanetType.Mars:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(marsMin, marsMax);
                position = mars.position + randomDirection * randomDistance;
                targetPlanetTransform = mars;
                enemiesOnMars++;
                break;
            
            case PlanetType.Jupiter:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(jupiterMin, jupiterMax);
                position = jupiter.position + randomDirection * randomDistance;
                targetPlanetTransform = jupiter;
                enemiesOnJupiter++;
                break;
            
            case PlanetType.Saturn:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(saturnMin, saturnMax);
                position = saturn.position + randomDirection * randomDistance;
                targetPlanetTransform = saturn;
                enemiesOnSaturn++;
                break;
            
            case PlanetType.Uranus:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(uranusMin, uranusMax);
                position = uranus.position + randomDirection * randomDistance;
                targetPlanetTransform = uranus;
                enemiesOnUranus++;
                break;
            
            case PlanetType.Neptune:
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(neptuneMin, neptuneMax);
                position = neptune.position + randomDirection * randomDistance;
                targetPlanetTransform = neptune;
                enemiesOnNeptune++;
                break;
        }
        var enemyGo = Instantiate(enemyPrefab, position, Quaternion.identity);
        var comp = enemyGo.GetComponent<Enemy>();
        comp.Initialize(type, targetPlanetTransform, planet);
    }
    
    public enum PlanetType
    {
        Mercury,
        Venus,
        Earth,
        Mars,
        Jupiter,
        Saturn,
        Uranus,
        Neptune
    }
}
