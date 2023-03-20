using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfoUI : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Awake()
    {
        _player.Finished += DisabledCarInfo;
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    private void DisabledCarInfo() 
    {
        gameObject.SetActive(false);
    }
}