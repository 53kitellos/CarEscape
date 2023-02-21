using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroPointer : MonoBehaviour
{
    [SerializeField] private ArrowIcon _arrowPrefab;
    [SerializeField] private Transform _carTransform;
    [SerializeField] private Camera _camera;

    private Dictionary<NitroSupply, ArrowIcon> _nitroObjects = new Dictionary<NitroSupply, ArrowIcon>();

    public static NitroPointer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddToList(NitroSupply nitro)
    {
        ArrowIcon newArrow = Instantiate(_arrowPrefab, transform);
        _nitroObjects.Add(nitro, newArrow);
    }

    public void RemoveFromList(NitroSupply nitro)
    {
        Destroy(_nitroObjects[nitro].gameObject);
        _nitroObjects.Remove(nitro);
    }

    private void LateUpdate()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        foreach (var currentNitro in _nitroObjects)
        {
            NitroSupply nitro = currentNitro.Key;
            ArrowIcon arrowIcon = currentNitro.Value;
            Vector3 toCar = nitro.transform.position - _carTransform.position;
            Ray ray = new Ray(_carTransform.position, toCar);
            Debug.DrawRay(_carTransform.position, toCar);
            float rayMinDistance = Mathf.Infinity;
            int index = 0;

            for (int i = 0; i < 4; i++)
            {
                if (planes[i].Raycast(ray, out float distance))
                {
                    if (distance < rayMinDistance)
                    {
                        rayMinDistance = distance;
                        index = i;
                    }
                }
            }

            rayMinDistance = Mathf.Clamp(rayMinDistance, 0, toCar.magnitude);
            Vector3 worldPosition = ray.GetPoint(rayMinDistance);
            Vector3 position = _camera.WorldToScreenPoint(worldPosition);
            Quaternion rotation = ArrowRotation(index);

            if (toCar.magnitude > rayMinDistance)
            {
                arrowIcon.Show();
            }
            else
            {
                arrowIcon.Hide();
            }

            arrowIcon.SetIconPosition(position, rotation);
        }

    }
    
    private Quaternion ArrowRotation(int planeIndex) 
    {
        if (planeIndex == 0)
            return Quaternion.Euler(0f,0f,90f);
        else if (planeIndex == 1)
            return Quaternion.Euler(0f, 0f, -90f);
        else if (planeIndex == 2)
            return Quaternion.Euler(0f, 0f, 180f);
        else if (planeIndex == 3)
            return Quaternion.Euler(0f, 0f, 0f);

        return Quaternion.identity;
    }
}