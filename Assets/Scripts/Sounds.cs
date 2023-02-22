using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource _loseSound;
    [SerializeField] private AudioSource _finishSound;
    [SerializeField] private AudioSource _engineSound;
    [SerializeField] private AudioSource _tyreScreechSound;
    [SerializeField] private AudioSource _mainSountrack;

    private void OnEnable()
    {
        _timer.TimeEnded += PlayLoseSound;
        _player.Finished += PlayFinishSound;
    }

    private void PlayLoseSound()
    {
        MuteMainSounds();
        _loseSound.Play();
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
        _mainSountrack.Stop();
    }
}