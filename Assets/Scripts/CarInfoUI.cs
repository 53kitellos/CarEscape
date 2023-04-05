using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfoUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Touchbuttons _touchButtons;

    private void Awake()
    {
        _player.Finished += DisabledCarInfo;

        if(PlayerPrefs.GetInt("touchpadOn") == 1)
            _touchButtons.gameObject.SetActive(true);
        else
            _touchButtons.gameObject.SetActive(false);
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