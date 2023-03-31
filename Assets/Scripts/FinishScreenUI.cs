using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FinishScreenUI : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void ShowExternRewardAdv();

    [DllImport("__Internal")]
    private static extern void SetInLeaderbord(float value);

    [SerializeField] private Player _player;
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioSource _placeRate;
    [SerializeField] private AudioSource _applause;
    [SerializeField] private PlaceIconPopUp _place1Image;
    [SerializeField] private PlaceIconPopUp _place2Image;
    [SerializeField] private PlaceIconPopUp _place3Image;
    [SerializeField] private Button _nextLVL;
    [SerializeField] private Button _openNextLVL;
    [SerializeField] private TMP_Text _finalTime;
    [SerializeField] private TMP_Text _bestTime;
    [SerializeField] public float FirstPlaceTime;
    [SerializeField] public float SecondPlaceTime;
    [SerializeField] public float ThirdPlaceTime;

    private bool _isOpenNextLevel = false;
    private int _medals = 0;

    private void Awake()
    {
        _player.Finished += ShowScreen;
        _openNextLVL.gameObject.SetActive(false);
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

        SetBestTime();

        _finalTime.text = ($" {Math.Round(_timer.FinalTime, 2)}");
        _bestTime.text = ($" {Math.Round(PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}"), 2)}"); 


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
            PlayerPrefs.SetInt("scenesOpened", SceneManager.GetActiveScene().buildIndex + 1);

            #if UNITY_WEBGL
            ProgressInfo.Instance.PlayerInfo.OpenedLevels = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log(ProgressInfo.Instance.PlayerInfo.OpenedLevels);
            #endif
        }
        else
        {
            yield return showingDelay;
            if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
                _openNextLVL.gameObject.SetActive(true);
        }

        Time.timeScale = 0;
    }

    private void SetMedalsNumber(int medals) 
    {
        PlayerPrefs.SetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}", medals);
    }

    private void SetBestTime() 
    {
        if (PlayerPrefs.HasKey($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}"))
        {
            if (_timer.FinalTime < PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}"))
            {
                PlayerPrefs.SetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}", _timer.FinalTime);
                //ProgressInfo.Instance.PlayerInfo.BestTime = _timer.FinalTime;
                //SetInLeaderbord(ProgressInfo.Instance.PlayerInfo.BestTime);
            }
        }
        else 
        {
            PlayerPrefs.SetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}", _timer.FinalTime);
            //ProgressInfo.Instance.PlayerInfo.BestTime = _timer.FinalTime;
            //SetInLeaderbord(ProgressInfo.Instance.PlayerInfo.BestTime);
        }

        //ProgressInfo.Instance.SavePlayerInfo();
    }

    public void LoadLevelsMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel() 
    {
        #if UNITY_WEBGL
        //ShowAdv();
        #endif

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void ShowRewardAdv() 
    {
        #if UNITY_WEBGL
        ShowExternRewardAdv();
        #endif

        PlayerPrefs.SetInt("scenesOpened", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }
}