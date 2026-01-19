using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TrackedType
{
    None,
    Neutral,
    Friendly,
    Enemy,
    Planet
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Tracker weaponTracker;
    [SerializeField] private Tracker[] planetTrackers;
    [SerializeField] private float trackerLerpMultiplier = 2f;
    [SerializeField] private float trackerTextAlpha = .2f;
    [SerializeField] private Image fuelFillBar;
    [SerializeField] private Image boostSymbol;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text corruptionText;
    [SerializeField] private Image corruptionFillBar;
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text causeOfDeathText;
    
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text sizeText;
    [SerializeField] private TMP_Text fireRateText;
    [SerializeField] private GameObject blueLaserObj;
    [SerializeField] private GameObject redLaserObj;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        gameOverPanel.SetActive(false);
    }

    public void SetWeaponTrackingInfo(Vector2 position, TrackedType type, string targetName = "", float targetDistance = 0f)
    {
        var sizeDelta = _rectTransform.sizeDelta;
        weaponTracker.SetWeaponTrackingInfo(sizeDelta, position, type, targetName, targetDistance);
    }

    public void SetFuelFillBar(float fillAmount)
    {
        fuelFillBar.fillAmount = fillAmount;
    }

    public void SetBoostSymbol(bool exhausted)
    {
        boostSymbol.DOColor(exhausted ? Color.red : Color.white, .2f);
    }

    public void UpdateEnemyCounts(int totalEnemies, int enemiesOnMercury, int enemiesOnVenus, int enemiesOnEarth, int enemiesOnMars,
        int enemiesOnJupiter, int enemiesOnSaturn, int enemiesOnUranus, int enemiesOnNeptune)
    {
        enemyCountText.text = $"<b><color=white>{totalEnemies}</color></b> Enemies:\n" +
                              $"<color=#800000ff>Mercury</color>: {enemiesOnMercury}\n" +
                              $"<color=orange>Venus</color>: {enemiesOnVenus}\n" +
                              $"<color=blue>Earth</color>: {enemiesOnEarth}\n" +
                              $"<color=red>Mars</color>: {enemiesOnMars}\n" +
                              $"<color=#c0c0c0ff>Jupiter</color>: {enemiesOnJupiter}\n" +
                              $"<color=yellow>Saturn</color>: {enemiesOnSaturn}\n" +
                              $"<color=#008080ff>Uranus</color>: {enemiesOnUranus}\n" +
                              $"<color=#00ffffff>Neptune</color>: {enemiesOnNeptune}";
    }
    
    public void UpdatePlanetTrackers(int[] planetEnemyCounts, Transform[] planets)
    {
        for (int i = 0; i < planetEnemyCounts.Length; i++)
        {
            var planetType = (EnemyManager.PlanetType)i;
            var planetTracker = planetTrackers[i];
            var enemyCount = planetEnemyCounts[i];
            var status = enemyCount > 0;
            planetTracker.gameObject.SetActive(status);
            
            if(!status) continue;
            
            // var screenPoint = mainCamera.WorldToViewportPoint(planets[i].position);
            
            var viewport = mainCamera.WorldToViewportPoint(planets[i].position); // x,y in [0..1], z distance
            Vector2 viewportPos = new Vector2(viewport.x, viewport.y);
            Vector2 center = new Vector2(0.5f, 0.5f);

            Vector2 screenPoint;
            // If the object is visible and inside the viewport, keep its position.
            bool isInside = viewport.z >= 0f && viewportPos.x >= 0f && viewportPos.x <= 1f && viewportPos.y >= 0f && viewportPos.y <= 1f;
            if (isInside)
            {
                screenPoint = viewportPos;
            }
            else
            {
                // Compute direction from screen center to the point.
                Vector2 dir = viewportPos - center;

                // If behind the camera, invert direction so the indicator points toward the object.
                if (viewport.z < 0f)
                    dir = -dir;

                // Avoid zero direction
                if (dir.sqrMagnitude < 1e-6f)
                    dir = Vector2.up * 0.0001f;

                // Project onto the viewport rectangle edge while keeping direction.
                float max = Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
                screenPoint = center + (dir / max) * 0.5f;

                // Ensure final coords are inside [0,1]
                screenPoint = new Vector2(Mathf.Clamp01(screenPoint.x), Mathf.Clamp01(screenPoint.y));
            }
            
            planetTracker.PlanetTracking(_rectTransform.sizeDelta, screenPoint, planetType, enemyCount);
        }
    }

    public void SetCorruptionDisplay(float corruption)
    {
        corruptionText.text = $"Hive Corruption <b><color=white>({Mathf.Ceil(corruption*100)}%)</color></b>";
        corruptionFillBar.fillAmount = corruption;
    }

    public void GameOver(string causeOfDeath)
    {
        causeOfDeathText.text = "Cause Of Death: " + causeOfDeath;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.SetActive(true);
    }

    public void ChangeScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void SetLaserWeapon(bool isBlue, float autoTargetRadius, float autoTargetMaxDistance, float fireRate)
    {
        blueLaserObj.SetActive(isBlue);
        redLaserObj.SetActive(!isBlue);
        rangeText.text = $"RANGE {autoTargetMaxDistance}";
        sizeText.text = $"SIZE {autoTargetRadius}";
        fireRateText.text = $"{fireRate:0.0}s COOLDOWN";
    }
}
