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

    public void ShowLevel(Level level) 
    {
        _levelImage.sprite = level.LevelImage;
        _levelNumber.text = level.LevelIndex.ToString();

        bool levelUlocked = PlayerPrefs.GetInt("scenesOpened",1) >= level.LevelIndex;
        _lockImage.SetActive(!levelUlocked);
        _startButton.interactable = levelUlocked;

        if (levelUlocked)
        {
            _levelImage.color = Color.white;
        }
        else 
        {
            _levelImage.color = Color.grey;
        }

        if (PlayerPrefs.GetInt($"medalsOnScene{level.LevelIndex}") == 1)
        {
            _bronzemedal.LiteItUp();
        }
        else if (PlayerPrefs.GetInt($"medalsOnScene{level.LevelIndex}") == 2)
        {
            _bronzemedal.LiteItUp();
            _silverMedal.LiteItUp();
        }
        else if (PlayerPrefs.GetInt($"medalsOnScene{level.LevelIndex}") == 3)
        {
            _bronzemedal.LiteItUp();
            _silverMedal.LiteItUp();
            _goldMedal.LiteItUp();
        }
        else 
        {
            _goldMedal.LiteItDown();
            _silverMedal.LiteItDown();
            _bronzemedal.LiteItDown();
        }

        _startButton.onClick.RemoveAllListeners();
        _startButton.onClick.AddListener(() => SceneManager.LoadScene(level.LoadScene.name));
    }
}