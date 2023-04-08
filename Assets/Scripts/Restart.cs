using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();

    public void ResetTheScene() 
    {
#if UNITY_WEBGL
        if (PlayerPrefs.GetInt("countAdvRestart", 1) == 1)
        {
            PlayerPrefs.SetInt("countAdvRestart", 2);
        }
        else if (PlayerPrefs.GetInt("countAdvRestart") == 8)
        {
            ShowAdv();
            PlayerPrefs.SetInt("countAdvRestart", 1);
        }
        else
        {
            PlayerPrefs.SetInt("countAdvRestart", PlayerPrefs.GetInt("countAdvRestart") + 1);
        }
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
