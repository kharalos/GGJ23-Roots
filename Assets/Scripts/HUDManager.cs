using System;
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
    Enemy
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private RectTransform tracker;
    [SerializeField] private Image trackerImage;
    [SerializeField] private TMP_Text trackerTmp;
    [SerializeField] private float trackerLerpMultiplier = 2f;
    [SerializeField] private float trackerTextAlpha = .2f;
    [SerializeField] private Image fuelFillBar;
    [SerializeField] private Image boostSymbol;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text corruptionText;
    [SerializeField] private Image corruptionFillBar;
    
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
        var targetPosition = new Vector2(position.x * sizeDelta.x - sizeDelta.x * 0.5f,
            position.y * sizeDelta.y - sizeDelta.y * 0.5f);

        tracker.anchoredPosition = Vector2.Lerp(
            tracker.anchoredPosition, targetPosition, Time.deltaTime * trackerLerpMultiplier);


        trackerTmp.text = type == TrackedType.None ?
            "" : $"{targetName} <color=#81FF88>{targetDistance:0.0}</color><color=white>m</color>";
        
        var targetColor = Color.white;
        switch (type)
        {
            case TrackedType.Neutral:
                targetColor = new Color(1f, 1f, 0.24f);
                break;
            case TrackedType.Friendly:
                targetColor = new Color(0.3f, 0.39f, 1f);
                break;
            case TrackedType.Enemy:
                targetColor = new Color(1f, 0.35f, 0.24f);
                break;
        }
        var targetSize = 1f;
        if (type != TrackedType.None)
        {
            targetSize = 1f - targetDistance / 100f;
            targetSize = Mathf.Clamp(targetSize, 0.3f, 1f);
        }
        
        trackerImage.transform.localScale = Vector3.Lerp(trackerImage.transform.localScale,
            Vector3.one * targetSize, Time.deltaTime * trackerLerpMultiplier);
        trackerImage.color = Color.Lerp(trackerImage.color, targetColor, Time.deltaTime * trackerLerpMultiplier);
        targetColor.a = trackerTextAlpha;
        trackerTmp.color = Color.Lerp(trackerTmp.color, targetColor, Time.deltaTime * trackerLerpMultiplier);
    }

    public void SetFuelFillBar(float fillAmount)
    {
        fuelFillBar.fillAmount = fillAmount;
    }

    public void SetBoostSymbol(bool exhausted)
    {
        boostSymbol.DOColor(exhausted ? Color.red : Color.white, .2f);
    }

    public void SetEnemyCountText(int totalEnemies, int enemiesOnMercury, int enemiesOnVenus, int enemiesOnEarth, int enemiesOnMars,
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

    public void SetCorruptionDisplay(float corruption)
    {
        corruptionText.text = $"Hive Corruption <b><color=white>({Mathf.Ceil(corruption*100)}%)</color></b>";
        corruptionFillBar.fillAmount = corruption;
    }

    public void GameOver(string causeOfDeath)
    {
        causeOfDeathText.text = "Cause Of Death:" + causeOfDeath;
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
