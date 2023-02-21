using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartPauseMenu : MonoBehaviour
{
    [SerializeField] private Sounds _sounds;

    private void Awake()
    {
        _sounds.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void CloseMenu(GameObject menu) 
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        _sounds.gameObject.SetActive(true);
    }

    public void OpenMenu(GameObject menu)
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        _sounds.gameObject.SetActive(false);
    }
}