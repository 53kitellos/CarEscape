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


    private void OnEnable()
    {
        _timer.TimeEnded += PlayLoseSound;
        _player.Finished += PlayFinishSound;
    }

    private void PlayLoseSound()
    {
        MuteCarSounds();
        _loseSound.Play();
    }
    private void PlayFinishSound()
    {
        MuteCarSounds();
        _finishSound.Play();
    }

    private void MuteCarSounds() 
    {
        _engineSound.Stop();
        _tyreScreechSound.Stop();
    }
}