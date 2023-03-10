using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScreenUI : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Awake()
    {
        _player.Finished += ShowScreen;
        gameObject.SetActive(false);
    }

    private void ShowScreen()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}