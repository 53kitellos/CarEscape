using TMPro;
using UnityEngine;

public class TimeForBronze : MonoBehaviour
{
    [SerializeField] private FinishScreenUI _finishScreenValues;
    [SerializeField] private TMP_Text _medalTime;

    private void Start()
    {
        _medalTime.text = _finishScreenValues.ThirdPlaceTime.ToString();
    }
}
