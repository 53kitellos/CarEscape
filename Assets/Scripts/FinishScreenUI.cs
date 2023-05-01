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
    private static extern void SetInLeaderbord(int value, int levelIndex);

    [SerializeField] private Player _player;
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioSource _placeRate;
    [SerializeField] private AudioSource _applause;
    [SerializeField] private PlaceIconPopUp _place1Image;
    [SerializeField] private PlaceIconPopUp _place2Image;
    [SerializeField] private PlaceIconPopUp _place3Image;
    [SerializeField] private Button _nextLVL;
    //[SerializeField] private Button _openNextLVL;
    [SerializeField] private TMP_Text _finalTime;
    [SerializeField] private TMP_Text _bestTime;
    [SerializeField] public float FirstPlaceTime;
    [SerializeField] public float SecondPlaceTime;
    [SerializeField] public float ThirdPlaceTime;

    private bool _isMedalGets = false;

    private void Awake()
    {
        _player.Finished += ShowScreen;
        //_openNextLVL.gameObject.SetActive(false);
        gameObject.SetActive(false);
        _nextLVL.interactable = false;
    }

    private void ShowScreen()
    {
        _applause.Play();   
        gameObject.SetActive(true);
        StopCoroutine(ShowPlaces());
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

            _isMedalGets = true;

            if (PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}") < 3)
            {
                SetMedalsNumber(3);
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

            _isMedalGets = true;

            if (PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}") < 2) 
            {
                SetMedalsNumber(2);
            }
        }
        else if (_timer.FinalTime <= ThirdPlaceTime && _timer.FinalTime > SecondPlaceTime) 
        {
            yield return showingDelay;
            _placeRate.Play();
            _place3Image.ShowIcon();

            _isMedalGets = true;

            if (PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}") < 1)
            {
                SetMedalsNumber(1);
            }
        }

        if (PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}") != 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                _nextLVL.interactable = false;
            }
            else 
            {
                _nextLVL.interactable = true;
            }
        }

        if (_isMedalGets)
        {
            PlayerPrefs.SetInt("scenesOpened", SceneManager.GetActiveScene().buildIndex + 1);

            #if UNITY_WEBGL
            ProgressInfo.Instance.PlayerInfo.OpenedLevels = SceneManager.GetActiveScene().buildIndex + 1;
            #endif
        }
        /*else
        {
            if (PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}") == 0) 
            {
                yield return showingDelay;

                if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
                    _openNextLVL.gameObject.SetActive(true);
            }
        }*/

#if UNITY_WEBGL
        Save();
#endif
        Time.timeScale = 0;
    }

    private void SetMedalsNumber(int medals) 
    {
        PlayerPrefs.SetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}", medals);
#if UNITY_WEBGL
        if (SceneManager.GetActiveScene().buildIndex == 1)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene1 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 2)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene2 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 3)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene3 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 4)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene4 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 5)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene5 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 6)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene6 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}"); 
        if (SceneManager.GetActiveScene().buildIndex == 7)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene7 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 8)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene8 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 9)
            ProgressInfo.Instance.PlayerInfo.medalsOnScene9 = PlayerPrefs.GetInt($"medalsOnScene{SceneManager.GetActiveScene().buildIndex}");
#endif
    }

    private void SetBestTime() 
    {
        if (PlayerPrefs.HasKey($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}"))
        {
            if (_timer.FinalTime < PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}"))
            {
                PlayerPrefs.SetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}", _timer.FinalTime);
                SetInLeaderbord( Mathf.CeilToInt(1000 * _timer.FinalTime), SceneManager.GetActiveScene().buildIndex);
            }
        }
        else 
        {
            PlayerPrefs.SetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}", _timer.FinalTime);
            SetInLeaderbord(Mathf.CeilToInt(1000 * _timer.FinalTime), SceneManager.GetActiveScene().buildIndex);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
            ProgressInfo.Instance.PlayerInfo.Level1BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 2)
            ProgressInfo.Instance.PlayerInfo.Level2BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 3)
            ProgressInfo.Instance.PlayerInfo.Level3BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 4)
            ProgressInfo.Instance.PlayerInfo.Level4BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 5)
            ProgressInfo.Instance.PlayerInfo.Level5BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 6)
            ProgressInfo.Instance.PlayerInfo.Level6BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 7)
            ProgressInfo.Instance.PlayerInfo.Level7BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 8)
            ProgressInfo.Instance.PlayerInfo.Level8BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
        if (SceneManager.GetActiveScene().buildIndex == 9)
            ProgressInfo.Instance.PlayerInfo.Level9BestTime = PlayerPrefs.GetFloat($"bestTimeOnScene{SceneManager.GetActiveScene().buildIndex}");
    }

    private void Save()
    {
        ProgressInfo.Instance.SavePlayerInfo();
    }

    public void LoadLevelsMenu()
    {
#if UNITY_WEBGL
        if (PlayerPrefs.GetFloat("countAdv", 1) % 2 == 1)
        {
            ShowAdv();
            PlayerPrefs.SetFloat("countAdv", 2);
        }
        else
        {
            PlayerPrefs.SetFloat("countAdv", 1);
        }

#endif
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel() 
    {
#if UNITY_WEBGL
        if (PlayerPrefs.GetFloat("countAdv", 1) % 2 == 1)
        {
            ShowAdv();
            PlayerPrefs.SetFloat("countAdv", 2);
        }
        else 
        {
            PlayerPrefs.SetFloat("countAdv", 1);
        }

#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    /*public void ShowRewardAdv() 
    {
        PlayerPrefs.SetInt("scenesOpened", SceneManager.GetActiveScene().buildIndex + 1);

#if UNITY_WEBGL
        ProgressInfo.Instance.PlayerInfo.OpenedLevels = SceneManager.GetActiveScene().buildIndex + 1;
        ShowExternRewardAdv();
        Save();
#endif

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }*/
}