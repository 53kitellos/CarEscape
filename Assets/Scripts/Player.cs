using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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