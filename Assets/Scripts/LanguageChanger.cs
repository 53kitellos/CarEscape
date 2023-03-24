using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private Sprite[] _flags;
    [SerializeField] private Button _currentLanguage;
    [SerializeField] private LeanLocalization _localizator;

    private int _flagIndex;

    private void Start()
    {
        if (_localizator.CurrentLanguage == "Russian")
        {
            _flagIndex = 0;
        }
        else if (_localizator.CurrentLanguage == "English")
        {
            _flagIndex = 1;
        }
        else if (_localizator.CurrentLanguage == "Turkish")
        {
            _flagIndex = 2;
        }

        _currentLanguage.image.sprite = _flags[_flagIndex];
    }

    public void SetNextLanguage() 
    {
        _flagIndex++;

        if (_flagIndex > _flags.Length - 1)
            _flagIndex = 0;

        _currentLanguage.image.sprite = _flags[_flagIndex];
    }
}