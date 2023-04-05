using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPadONOFF : MonoBehaviour
{
    [SerializeField] private Toggle _touchpad;

    private void Start()
    {
        if(PlayerPrefs.GetInt("touchpadOn")==1)
            _touchpad.isOn = true;
        else
            _touchpad.isOn = false;
    }

    public void SwitchTouchPad() 
    {
        if (_touchpad.isOn)
        {
            PlayerPrefs.SetInt("touchpadOn", 1);
        }
        else 
        {
            PlayerPrefs.SetInt("touchpadOn", 0);
        }
    }
}