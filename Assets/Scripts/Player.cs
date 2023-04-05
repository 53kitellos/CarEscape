using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private bool _canShowAdv;
    public event Action Finished;

    public void OnTriggerEnter(Collider collision)
    {      
        if (collision.TryGetComponent<FinishLine>(out FinishLine finish))
        {
            Finished?.Invoke();
        }
    }

    
    private void FixedUpdate()
    {
        PlayerPrefs.SetFloat("currentTimer", PlayerPrefs.GetFloat("currentTimer", 60) - Time.deltaTime);

        if (PlayerPrefs.GetFloat("currentTimer") <= 0)
            _canShowAdv = true;
    }

    public bool CanShowAdv() 
    {
        return _canShowAdv;
    }
}