using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YandexCommands : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    //[SerializeField] private Button _rateButton;
 
    [DllImport("__Internal")]
    private static extern void SetPlayerData();

    [DllImport("__Internal")]
    private static extern void AskGameFeedback();

    [DllImport("__Internal")]
    private static extern string InitUserDevice();

    public string UserDevice;

    private void Awake()
    {
        #if UNITY_WEBGL
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            UserDevice = InitUserDevice();

            if (UserDevice == "desktop")
            {
                PlayerPrefs.SetInt("touchpadOn", 0);
            }
            else if (UserDevice == "mobile")
            {
                PlayerPrefs.SetInt("touchpadOn", 1);
            }
        }
#endif
    }

    public void GetFeedback()
    {
        AskGameFeedback();
    }

    public void GetPlayerInfo() 
    {
        SetPlayerData();
    }

    public void SetName(string name) 
    {
        _playerName.text = name;
    }
}