using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField] AudioSource _click;
    public void PlaySound() 
    {
        StartCoroutine(ClickSound());
    }

    IEnumerator ClickSound() 
    {
        _click.Play();
        yield return new WaitForSeconds(0.1f);
    }
}

