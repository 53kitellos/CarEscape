using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroSpawner : MonoBehaviour
{
    [SerializeField] private NitroSupply _nitro;
    [SerializeField] private Vector3[] _spawnPoints;

    private int _currentPointIndex = 0;

    private void Start()
    {
        NitroSupply newNitro = Instantiate(_nitro, _spawnPoints[_currentPointIndex++], transform.rotation);
    }

    private void Update()
    {
        NitroSupply[] nitroSupplies = FindObjectsOfType<NitroSupply>();

        if (nitroSupplies.Length < 2)
        {
            NitroSupply newNitro = Instantiate(_nitro, _spawnPoints[_currentPointIndex++], transform.rotation);

            if (_currentPointIndex == _spawnPoints.Length-1) 
            {
                _currentPointIndex = 0;
            }
        }
    }
}