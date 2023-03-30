using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource _finishSound;
    [SerializeField] private AudioSource _engineSound;
    [SerializeField] private AudioSource _tyreScreechSound;
    [SerializeField] private AudioSource _mainSountrack;

    private void OnEnable()
    {
        _player.Finished += PlayFinishSound;
    }

    private void PlayLoseSound()
    {
        MuteMainSounds();
    }

    private void PlayFinishSound()
    {
        MuteMainSounds();
        _finishSound.Play();
    }

    private void MuteMainSounds() 
    {
        _engineSound.Stop();
        _tyreScreechSound.Stop();
        _tyreScreechSound.volume = 0;
        _mainSountrack.Stop();
    }
}