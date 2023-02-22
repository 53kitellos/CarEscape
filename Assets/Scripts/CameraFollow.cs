using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform _carTransform;
	[Range(1, 10)]
	private float _followSpeed = 2;
	[Range(1, 10)]
	private float _lookSpeed = 5;
	private Vector3 _initialCameraPosition;
    private Vector3 _initialCarPosition;
	private Vector3 _absoluteInitCameraPosition;

	void Start()
	{
		_initialCameraPosition = gameObject.transform.position;
		_initialCarPosition = _carTransform.position;
		_absoluteInitCameraPosition = _initialCameraPosition - _initialCarPosition;
	}

	void FixedUpdate()
	{
		Vector3 lookDirection = (new Vector3(_carTransform.position.x, _carTransform.position.y, _carTransform.position.z)) - transform.position;
		Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rot, _lookSpeed * Time.deltaTime);

		Vector3 targetPos = _absoluteInitCameraPosition + _carTransform.transform.position;
		transform.position = Vector3.Lerp(transform.position, targetPos, _followSpeed * Time.deltaTime);
	}
}