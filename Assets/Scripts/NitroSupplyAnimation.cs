using UnityEngine;
using System.Collections;

public class NitroSupplyAnimation : MonoBehaviour
{
    private Vector3 _rotationAngle = new Vector3(0,0,10);
    private Vector3 startScale = new Vector3(1,1,1);
    private Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);
    private float _rotationSpeed = 10;
    private float _scaleSpeed = 1;
    private float scaleRate = 0.5f;
    private float _scaleTimer;
    private bool scalingUp = true;

    void LateUpdate ()
    {
        transform.Rotate(_rotationAngle * _rotationSpeed * Time.deltaTime);
        _scaleTimer += Time.deltaTime;

        if (scalingUp)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, _scaleSpeed * Time.deltaTime);
        }
        else if (!scalingUp)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, _scaleSpeed * Time.deltaTime);
        }

        if (_scaleTimer >= scaleRate)
        {
            if (scalingUp)
            {
                scalingUp = false;
            }
            else if (!scalingUp)
            {
                scalingUp = true;
            }

            _scaleTimer = 0;
        }
    }
}