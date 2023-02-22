using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NitroBar : MonoBehaviour
{
    [SerializeField] private PlayerCarControl _car;
    [SerializeField] private Slider _nitroBar;

    private float _currentNitroValue = 0;

    public void OnEnable()
    {
        _car.NitroValueChanged += OnNitroValueChange;
        _nitroBar.value = 0;
    }

    public void OnDisable()
    {
        _car.NitroValueChanged -= OnNitroValueChange;
    }

    public void OnNitroValueChange(float value, float maxValue) 
    {
        _currentNitroValue = Mathf.Clamp(_currentNitroValue+value,0,100);
        _nitroBar.value = _currentNitroValue / maxValue;
    }
   
    public bool TryGetNitro() 
    {
        return _currentNitroValue > 0;
    }
}