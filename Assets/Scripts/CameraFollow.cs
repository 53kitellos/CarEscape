using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform carTransform;
	[Range(1, 10)]
	public float followSpeed = 2;
	[Range(1, 10)]
	public float lookSpeed = 5;
	Vector3 initialCameraPosition;
	Vector3 initialCarPosition;
	Vector3 absoluteInitCameraPosition;

	void Start()
	{
		initialCameraPosition = gameObject.transform.position;
		initialCarPosition = carTransform.position;
		absoluteInitCameraPosition = initialCameraPosition - initialCarPosition;
	}

	void FixedUpdate()
	{
		Vector3 lookDirection = (new Vector3(carTransform.position.x, carTransform.position.y, carTransform.position.z)) - transform.position;
		Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);

		Vector3 targetPos = absoluteInitCameraPosition + carTransform.transform.position;
		transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
	}
}