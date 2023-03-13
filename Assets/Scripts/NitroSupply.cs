using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroSupply : MonoBehaviour
{
    private float _lifeTime = 3;

    private void Start()
    {
        NitroPointer.Instance.AddToList(this);
       // StartCoroutine(LifeTime());
    }

    public void SelfDetroy() 
    {
        NitroPointer.Instance.RemoveFromList(this);
        Destroy(gameObject);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        SelfDetroy();
    }
}