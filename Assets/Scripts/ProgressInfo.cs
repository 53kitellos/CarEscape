using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerInfo 
{
    public int OpenedLevels;
}

public class ProgressInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerInfoText;

    public PlayerInfo PlayerInfo;

    [DllImport("__Internal")]
    private static extern void SaveInfoExtern(string date);

    [DllImport("__Internal")]
    private static extern void LoadInfoExtern();

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

        //_playerInfoText.text = PlayerInfo.BestTime + "\n" + PlayerInfo.OpenedLevels;
        _playerInfoText.text = PlayerInfo.OpenedLevels.ToString();
    }
}
