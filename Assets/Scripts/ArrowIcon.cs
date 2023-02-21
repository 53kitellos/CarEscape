using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowIcon : MonoBehaviour
{
    [SerializeField] private Image _image;

    private bool _isShown = true;

    private void Awake()
    {
        _image.enabled = false;
        _isShown = false;
    }

    public void SetIconPosition(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Show()
    {
        if (_isShown)
            return;
        
        _isShown = true;
        StopAllCoroutines();
        StartCoroutine(ShowProcess());
    }

    public void Hide()
    {
        if (!_isShown)
            return;
       
        _isShown = false;
        StopAllCoroutines();
        StartCoroutine(HideProcess());
    }

    private IEnumerator ShowProcess()
    {
        _image.enabled = true;
        transform.localScale = Vector3.zero;

        for (float i = 0; i < 1f; i += Time.deltaTime * 4f)
        {
            transform.localScale = Vector3.one * i;
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    private IEnumerator HideProcess() 
    {
        for (float i = 0; i < 1f; i += Time.deltaTime * 4f)
        {
            transform.localScale = Vector3.one * (1f - i);
            yield return null;
        }

        _image.enabled = false;
    }
}