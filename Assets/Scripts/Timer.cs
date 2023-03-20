using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _timerValue;
    [SerializeField] private Player _player;

    private float _currentTime = 0;
    public float FinalTime { get; private set; }

    private void Start()
    {
        _player.Finished += SetFinalTime;

        if (_timerValue != null)
        {
            _timerValue.text = _currentTime.ToString();
        }
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        _timerValue.text = Math.Round(_currentTime, 2).ToString();
    }

    private void SetFinalTime() 
    {
        FinalTime = _currentTime;
    }

    
}