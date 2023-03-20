using UnityEngine;
using UnityEngine.UI;

public class Medal : MonoBehaviour
{
    [SerializeField] private Image _medal; 

    public void LiteItUp() 
    {
        _medal.color = Color.white;
    }

    public void LiteItDown()
    {
        _medal.color = Color.grey;
    }
}