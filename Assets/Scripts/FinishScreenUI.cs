using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FinishScreenUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioSource _placeRate;
    [SerializeField] private AudioSource _applause;
    [SerializeField] private PlaceIconPopUp _place1Image;
    [SerializeField] private PlaceIconPopUp _place2Image;
    [SerializeField] private PlaceIconPopUp _place3Image;
    [SerializeField] private Button _nextLVL;
    [SerializeField] private Text _finalTime;
    [SerializeField] public float FirstPlaceTime;
    [SerializeField] public float SecondPlaceTime;
    [SerializeField] public float ThirdPlaceTime;

    private bool _isOpenNextLevel = false;
    private int _medals = 0;

    private void Awake()
    {
        _player.Finished += ShowScreen;
        gameObject.SetActive(false);
        _nextLVL.interactable = false;
    }

    private void ShowScreen()
    {
        _applause.Play();   
        gameObject.SetActive(true);
        StartCoroutine(ShowPlaces());
    }

    private IEnumerator ShowPlaces()
    {
        yield return new WaitForSeconds(0.1f);
        var showingDelay = new WaitForSeconds(1);

        _finalTime.text = ($"TIME: {Math.Round(_timer.FinalTime, 2)}");

        if (_timer.FinalTime <= FirstPlaceTime)
        {
            yield return showingDelay;
            _placeRate.Play();
            _place3Image.ShowIcon();

            yield return showingDelay;
            _placeRate.Play();
            _place2Image.ShowIcon();

            yield return showingDelay;
            _placeRate.Play();
            _place1Image.ShowIcon();

            _isOpenNextLevel = true;

            if (_medals < 3)
            {
                _medals = 3;
                SetMedalsNumber(_medals);
            }
        }
        else if (_timer.FinalTime <= SecondPlaceTime && _timer.FinalTime > FirstPlaceTime)
        {
            yield return showingDelay;
            _placeRate.Play();
            _place3Image.ShowIcon();
            yield return showingDelay;
            _placeRate.Play();
            _place2Image.ShowIcon();

            _isOpenNextLevel = true;

            if (_medals < 2) 
            {
                _medals = 2;
                SetMedalsNumber(_medals);
            }
        }
        else if (_timer.FinalTime <= ThirdPlaceTime && _timer.FinalTime > SecondPlaceTime) 
        {
            yield return showingDelay;
            _placeRate.Play();
            _place3Image.ShowIcon();

            _isOpenNextLevel = true;

            if (_medals < 1)
            {
                _medals = 1;
                SetMedalsNumber(_medals);
            }
        }

        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
            _nextLVL.interactable = _isOpenNextLevel;

        if (_isOpenNextLevel) 
        {
            PlayerPrefs.SetInt("scenesOpened", SceneManager.GetActiveScene().buildIndex+1);
        }

        Time.timeScale = 0;
    }

    private void SetMedalsNumber(int medals) 
    {
        PlayerPrefs.SetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}", medals);
    }

    public void LoadLevelsMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }
}