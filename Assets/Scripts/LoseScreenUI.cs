using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreenUI : MonoBehaviour
{
    [SerializeField] private Timer _timer;

    private void ShowLoseScreen()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void Awake()
    {
        _timer.TimeEnded += ShowLoseScreen;
        gameObject.SetActive(false);
    }
}