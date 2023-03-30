using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelView : MonoBehaviour
{
    [SerializeField] private Image _levelImage;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _lockImage;
    [SerializeField] private Text _levelNumber;
    [SerializeField] private Medal _goldMedal;
    [SerializeField] private Medal _silverMedal;
    [SerializeField] private Medal _bronzemedal;

    private int _currentIndex;

    public void ShowLevel(Level level) 
    {
        _currentIndex = level.LevelIndex;
        _levelImage.sprite = level.LevelImage;
        _levelNumber.text = level.LevelIndex.ToString();

        bool levelUlocked = PlayerPrefs.GetInt("scenesOpened", 1) >= level.LevelIndex;
        //ProgressInfo.Instance.PlayerInfo.OpenedLevels = PlayerPrefs.GetInt("scenesOpened", 1);

        _lockImage.SetActive(!levelUlocked);
        _startButton.interactable = levelUlocked;

        if (levelUlocked)
            _levelImage.color = Color.white;
        else 
            _levelImage.color = Color.grey;

        if (PlayerPrefs.GetInt($"medalsOnScene{level.LevelIndex}") == 1)
        {
            _bronzemedal.LiteItUp();
            _silverMedal.LiteItDown();
            _goldMedal.LiteItDown();
        }
        else if (PlayerPrefs.GetInt($"medalsOnScene{level.LevelIndex}") == 2)
        {
            _bronzemedal.LiteItUp();
            _silverMedal.LiteItUp();
            _goldMedal.LiteItDown();
        }
        else if (PlayerPrefs.GetInt($"medalsOnScene{level.LevelIndex}") == 3)
        {
            _bronzemedal.LiteItUp();
            _silverMedal.LiteItUp();
            _goldMedal.LiteItUp();
        }
        else 
        {
            _bronzemedal.LiteItDown();
            _silverMedal.LiteItDown();
            _goldMedal.LiteItDown();
        }
    }

    public void LoadCurrentLevel() 
    {
        SceneManager.LoadScene(_currentIndex);
    }
}