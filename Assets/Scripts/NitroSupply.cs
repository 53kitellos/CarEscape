using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroSupply : MonoBehaviour
{
    private float _lifeTime = 3;
    private float _currentLifeTime;

    private void Start()
    {
        NitroPointer.Instance.AddToList(this);
    }

    private void Update()
    {
        if (_currentLifeTime <= _lifeTime)
        {
            _currentLifeTime += Time.deltaTime;
        }
        else 
        {
            SelfDetroy();
        }
    }

    public void SelfDetroy() 
    {
        NitroPointer.Instance.RemoveFromList(this);
        Destroy(gameObject);
    } 
}