using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event Action Finished;

    public void OnTriggerEnter(Collider collision)
    {      
        if (collision.TryGetComponent<FinishLine>(out FinishLine finish))
        {
            Finished?.Invoke();
        }
    }
}