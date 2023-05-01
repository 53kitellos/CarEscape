using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

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
    public int medalsOnScene1 ;
    public int medalsOnScene2 ;
    public int medalsOnScene3 ; 
    public int medalsOnScene4 ;
    public int medalsOnScene5 ; 
    public int medalsOnScene6 ; 
    public int medalsOnScene7 ;
    public int medalsOnScene8 ;
    public int medalsOnScene9 ;

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

        PlayerPrefs.SetInt("medalsOnScene1", PlayerInfo.medalsOnScene1);
        PlayerPrefs.SetInt("medalsOnScene2", PlayerInfo.medalsOnScene2);
        PlayerPrefs.SetInt("medalsOnScene3", PlayerInfo.medalsOnScene3);
        PlayerPrefs.SetInt("medalsOnScene4", PlayerInfo.medalsOnScene4);
        PlayerPrefs.SetInt("medalsOnScene5", PlayerInfo.medalsOnScene5);
        PlayerPrefs.SetInt("medalsOnScene6", PlayerInfo.medalsOnScene6);
        PlayerPrefs.SetInt("medalsOnScene7", PlayerInfo.medalsOnScene7);
        PlayerPrefs.SetInt("medalsOnScene8", PlayerInfo.medalsOnScene8);
        PlayerPrefs.SetInt("medalsOnScene9", PlayerInfo.medalsOnScene9);
    }
}