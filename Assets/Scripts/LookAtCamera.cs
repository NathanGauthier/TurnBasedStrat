using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	[SerializeField] private bool invert;

	private Transform cameraTansform;

	private void Awake()
	{
		cameraTansform = Camera.main.transform;
	}

	private void LateUpdate()
	{
		if(invert)
		{
			Vector3 dirToCam = (cameraTansform.position - transform.position).normalized;
			transform.LookAt(transform.position + (-dirToCam));
		}
		else
		{
			transform.LookAt(cameraTansform.position);
		}
		
	}
}
