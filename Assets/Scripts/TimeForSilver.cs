using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeForSilver : MonoBehaviour
{
    [SerializeField] private FinishScreenUI _finishScreenValues;
    [SerializeField] private TMP_Text _medalTime;

    private void Start()
    {
        _medalTime.text = _finishScreenValues.SecondPlaceTime.ToString();
    }
}
