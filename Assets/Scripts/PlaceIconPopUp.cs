using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceIconPopUp : MonoBehaviour
{
    [SerializeField] private Image PlaceIcon;

    public void ShowIcon()
    {
        PlaceIcon.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        PlaceIcon.color = new Color(255,255,255);
    }
}
