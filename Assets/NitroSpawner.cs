using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroSpawner : MonoBehaviour
{
    [SerializeField] private NitroSupply _nitro;
    [SerializeField] private Vector3[] _spawnPoints;


    private void Start()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            NitroSupply newNitro = Instantiate(_nitro, _spawnPoints[i], transform.rotation);
        }
    }
}
