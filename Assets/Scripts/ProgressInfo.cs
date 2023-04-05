using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerInfo 
{
    public int OpenedLevels = 1;
    public float Level1BestTime = 100;
    public float Level2BestTime = 100;
    public float Level3BestTime = 100;
    public float Level4BestTime = 100;
    public float Level5BestTime = 100;
    public float Level6BestTime = 100;
    public float Level7BestTime = 100;
    public float Level8BestTime = 100;
    public float Level9BestTime = 100;
}

public class ProgressInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerInfoText;

    [DllImport("__Internal")]
    private static extern void SaveInfoExtern(string date);

    [DllImport("__Internal")]
    private static extern void LoadInfoExtern();

    public PlayerInfo PlayerInfo;
    public static ProgressInfo Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;

            #if UNITY_WEBGL
            LoadInfoExtern();
            #endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerInfo()
    {
        string jsonString = JsonUtility.ToJson(PlayerInfo);
        SaveInfoExtern(jsonString);
    }

    public void LoadPlayerInfo(string value)
    {
        PlayerInfo = JsonUtility.FromJson<PlayerInfo>(value);

        PlayerPrefs.SetInt("scenesOpened", PlayerInfo.OpenedLevels);

        PlayerPrefs.SetFloat("bestTimeOnScene1", PlayerInfo.Level1BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene2", PlayerInfo.Level2BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene3", PlayerInfo.Level3BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene4", PlayerInfo.Level4BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene5", PlayerInfo.Level5BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene6", PlayerInfo.Level6BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene7", PlayerInfo.Level7BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene8", PlayerInfo.Level8BestTime);
        PlayerPrefs.SetFloat("bestTimeOnScene9", PlayerInfo.Level9BestTime);

        //_playerInfoText.text = PlayerInfo.OpenedLevels + "\n" + PlayerInfo.Level1BestTime + "\n" + PlayerInfo.Level2BestTime + "\n" + PlayerInfo.Level3BestTime + "\n" + PlayerInfo.Level4BestTime + "\n" + PlayerInfo.Level5BestTime + "\n" + PlayerInfo.Level6BestTime + "\n" + PlayerInfo.Level7BestTime + "\n" + PlayerInfo.Level8BestTime + "\n" + PlayerInfo.Level9BestTime ;
    }
}
