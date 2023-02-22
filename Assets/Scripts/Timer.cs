using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _timeLeft;
    private float _roundTime = 45;
    private bool _isEnded = false;

    public event UnityAction TimeEnded;

    private void Start()
    {
        if (_timeLeft != null)
        {
            _timeLeft.text = _roundTime.ToString();
        }
    }

    private void Update()
    {
        if (_isEnded == false)
        {
            _roundTime -= Time.deltaTime;
            _timeLeft.text = Mathf.RoundToInt(_roundTime).ToString();

            if (_roundTime <= 0)
            {
                _isEnded = true;
                TimeEnded?.Invoke();
            }
        }
    }
}