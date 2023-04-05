using Lean.Localization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetLang();

    [SerializeField] private Sprite[] _flags;
    [SerializeField] private Button _currentLanguage;
    [SerializeField] private LeanLocalization _localizator;

    private int _flagIndex;
    public string UserLanguage;
  
    private void Awake()
    {
#if UNITY_WEBGL
        UserLanguage = GetLang();

        if (PlayerPrefs.GetInt("isLanguageInditfied",0) == 0 ) 
        {
            SetUserLanguage();
            PlayerPrefs.SetInt("isLanguageInditfied", 1);
        }
        
#endif
    }

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

    public void SetUserLanguage() 
    {
        if (UserLanguage == "ru")
        {
            _localizator.SetCurrentLanguage("Russian");
        }
        else if (UserLanguage == "en")
        {
            _localizator.SetCurrentLanguage("English");
        }
        else if (UserLanguage == "tr")
        {
            _localizator.SetCurrentLanguage("Turkish");
        }
    }
}