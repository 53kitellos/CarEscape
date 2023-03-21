using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundONOFF : MonoBehaviour
{
    [SerializeField] private Button _onOffButton;
    [SerializeField] private Sprite _soundOnIcon;
    [SerializeField] private Sprite _soundOffIcon;

    private bool _isOn = true;

    public void ChangeVolume() 
    {
        if (_isOn)
        {
            _onOffButton.image.sprite = _soundOffIcon;
            AudioListener.volume = 0f;
            _isOn = false;
        }
        else 
        {
            _onOffButton.image.sprite = _soundOnIcon;
            AudioListener.volume = 1f;
            _isOn = true;
        }
    }
}