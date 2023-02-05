using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlsWindow;
    private bool _controlsWindowActive = false;

    private void Start()
    {
        controlsWindow.SetActive(false);
        controlsWindow.transform.localScale = Vector3.zero;
        _controlsWindowActive = false;
        Time.timeScale = 1;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Controls()
    {
        controlsWindow.transform.DOKill();
        if (_controlsWindowActive)
        {
            controlsWindow.transform.DOScale(0f, 0.5f).OnComplete(() => controlsWindow.SetActive(false));
            _controlsWindowActive = false;
        }
        else
        {
            controlsWindow.SetActive(true);
            controlsWindow.transform.DOScale(1, 0.5f);
            _controlsWindowActive = true;
        }
    }
}
