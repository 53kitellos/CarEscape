using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class YandexCommands : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private RawImage _playerIcon;
    [SerializeField] private Button _rateButton;

    private bool _isRateDone = false; 

    [DllImport("__Internal")]
    private static extern void SetPlayerData();

    [DllImport("__Internal")]
    private static extern void AskGameFeedback();

    public void GetFeedback()
    {
        AskGameFeedback();

        if (_isRateDone)
        {
            _rateButton.gameObject.SetActive(false);
        }

    }

    public void GetPlayerInfo() 
    {
        SetPlayerData();
    }

    public void SetName(string name) 
    {
        _playerName.text = name;
    }

    public void SetIcon(string URL) 
    {
        StartCoroutine(DownLoadImage(URL));
    }

    public void RateDone(bool isDone) 
    {
        _isRateDone = isDone;
    }

    private IEnumerator DownLoadImage(string mediaURL)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            _playerIcon.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}
