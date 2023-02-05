using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
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
    [SerializeField] private string[] planets = new string[8] {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
    
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

    private int _enemyTypeCount;

    public void EnemyDestroyed(string targetTag)
    {
        switch (targetTag)
        {
            case "Mercury":
                enemiesOnMercury--;
                break;
            case "Venus":
                enemiesOnVenus--;
                break;
            case "Earth":
                enemiesOnEarth--;
                break;
            case "Mars":
                enemiesOnMars--;
                break;
            case "Jupiter":
                enemiesOnJupiter--;
                break;
            case "Saturn":
                enemiesOnSaturn--;
                break;
            case "Uranus":
                enemiesOnUranus--;
                break;
            case "Neptune":
                enemiesOnNeptune--;
                break;
        }
        currentEnemyCount--;
        _corruption -= 0.1f;
        hudManager.SetEnemyCountText(currentEnemyCount, enemiesOnMercury, enemiesOnVenus,
            enemiesOnEarth, enemiesOnMars, enemiesOnJupiter, enemiesOnSaturn, enemiesOnUranus, enemiesOnNeptune);
    }

    private void Start()
    {
        enemiesOnMercury = 0;
        enemiesOnVenus = 0;
        enemiesOnEarth = 0;
        enemiesOnMars = 0;
        enemiesOnJupiter = 0;
        enemiesOnSaturn = 0;
        enemiesOnUranus = 0;
        enemiesOnNeptune = 0;
        
        _enemyTypeCount = Enum.GetNames(typeof(EnemyType)).Length;
        StartCoroutine(SpawnEnemies());
        
        Time.timeScale = 1;
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnRandomEnemy();
        }
        hudManager.SetEnemyCountText(currentEnemyCount, enemiesOnMercury, enemiesOnVenus,
            enemiesOnEarth, enemiesOnMars, enemiesOnJupiter, enemiesOnSaturn, enemiesOnUranus, enemiesOnNeptune);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
            
            if (currentEnemyCount < maxEnemyCount)
            {
                SpawnRandomEnemy();
                hudManager.SetEnemyCountText(currentEnemyCount, enemiesOnMercury, enemiesOnVenus,
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
    }

    private void SpawnRandomEnemy()
    {
        currentEnemyCount++;
        var planet = planets[Random.Range(0, planets.Length)];
        var enemyType = (EnemyType) Random.Range(0, _enemyTypeCount);
        SpawnEnemy(planet, enemyType);
    }

    private void SpawnEnemy(string planet, EnemyType type)
    {
        switch (planet)
        {
            case "Mercury":
                Vector3 randomDirection = Random.onUnitSphere;
                float randomDistance = Random.Range(mercuryMin, mercuryMax);
                Vector3 randomPosition = mercury.position + randomDirection * randomDistance;
                
                var enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                var enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, mercury, "Mercury");
                enemiesOnMercury++;
                break;
            
            case "Venus":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(venusMin, venusMax);
                randomPosition = venus.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, venus, "Venus");
                enemiesOnVenus++;
                break;
            
            case "Earth":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(earthMin, earthMax);
                randomPosition = earth.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, earth, "Earth");
                enemiesOnEarth++;
                break;
            
            case "Mars":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(marsMin, marsMax);
                randomPosition = mars.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, mars, "Mars");
                enemiesOnMars++;
                break;
            
            case "Jupiter":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(jupiterMin, jupiterMax);
                randomPosition = jupiter.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, jupiter, "Jupiter");
                enemiesOnJupiter++;
                break;
            
            case "Saturn":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(saturnMin, saturnMax);
                randomPosition = saturn.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, saturn, "Saturn");
                enemiesOnSaturn++;
                break;
            
            case "Uranus":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(uranusMin, uranusMax);
                randomPosition = uranus.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, uranus, "Uranus");
                enemiesOnUranus++;
                break;
            
            case "Neptune":
                randomDirection = Random.onUnitSphere;
                randomDistance = Random.Range(neptuneMin, neptuneMax);
                randomPosition = neptune.position + randomDirection * randomDistance;
                
                enemyGo = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                enemy = enemyGo.GetComponent<Enemy>();
                enemy.Initialize(type, neptune, "Neptune");
                enemiesOnNeptune++;
                break;
        }
    }
}
