using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
	private const float MIN_FOLLOW_Y_OFFSET = 2f;
	private const float MAX_FOLLOW_Y_OFFSET = 12f;
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

	private Vector3 targetFollowOffset;
	CinemachineTransposer cinemachineTransposer;

	private void Start()
	{
		cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
		targetFollowOffset = cinemachineTransposer.m_FollowOffset;
	}

	// Update is called once per frame
	void Update()
    {
		HandleMovement();
		HandleRotation();
		HandleZoom();
	}

	private void HandleMovement()
	{
		Vector3 inputMoveDirection = new Vector3(0, 0, 0);

		if (Input.GetKey(KeyCode.Z))
		{
			inputMoveDirection.z = +1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			inputMoveDirection.z = -1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			inputMoveDirection.x = +1f;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			inputMoveDirection.x = -1f;
		}

		float moveSpeed = 10f;
		Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
		transform.position += moveVector * moveSpeed * Time.deltaTime;
	}

	private void HandleRotation()
	{

		Vector3 rotationVector = new Vector3(0, 0, 0);

		if (Input.GetKey(KeyCode.A))
		{
			rotationVector.y = +1f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			rotationVector.y = -1f;
		}


		float rotationSpeed = 100f;

		transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
	}

	private void HandleZoom()
	{
		float zoomAmount = 1f;

		if (Input.mouseScrollDelta.y > 0)
		{
			targetFollowOffset.y -= zoomAmount;
		}

		if (Input.mouseScrollDelta.y < 0)
		{
			targetFollowOffset.y += zoomAmount;
		}

		targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

		float zoomSpeed = 5f;
		cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
	}
}
