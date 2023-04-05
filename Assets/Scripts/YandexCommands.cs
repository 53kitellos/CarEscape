using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YandexCommands : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private Button _rateButton;
 
    [DllImport("__Internal")]
    private static extern void SetPlayerData();

    [DllImport("__Internal")]
    private static extern void AskGameFeedback();

    private void Awake()
    {
        if (PlayerPrefs.GetInt("RateIsDone", 0) == 1) 
        {
            _rateButton.gameObject.SetActive(false);
        } 
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

    public void RateDone(bool isDone) 
    {
        if (isDone)
            PlayerPrefs.SetInt("RateIsDone", 1);
    }

    /*
    private IEnumerator DownLoadImage(string mediaURL)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            _playerIcon.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }*/
}
