using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPointer : MonoBehaviour
{
    [SerializeField] Transform _carTransform;
    [SerializeField] Transform _arrowPoint;
    [SerializeField] Camera _camera;

    private void Update()
    {
        Vector3 fromNitroToEnemy = transform.position - _carTransform.position;
        Ray ray = new Ray(_carTransform.position, fromNitroToEnemy);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < planes.Length; i++)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

        }

        minDistance = Mathf.Clamp(minDistance,0, fromNitroToEnemy.magnitude);

        Vector3 worldPosition = ray.GetPoint(minDistance);

        _arrowPoint.position = _camera.WorldToScreenPoint(worldPosition);
    }
}
