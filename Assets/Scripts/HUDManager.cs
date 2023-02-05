using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Image fuelFillBar;
    [SerializeField] private Image boostSymbol;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text corruptionText;
    [SerializeField] private Image corruptionFillBar;
    
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text causeOfDeathText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
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
}
