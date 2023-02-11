using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] private GameObject actionCamGameObject;

	private void Start()
	{
		BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
		BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

		Hide();
	}

	private void Show()
	{
		actionCamGameObject.SetActive(true);
	}

	private void Hide()
	{
		actionCamGameObject.SetActive(false) ;
	}

	private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
	{
		switch (sender)
		{
			case ShootAction shootAction:
				Unit shooterUnit = shootAction.GetUnit();
				Unit targetUnit = shootAction.GetTargetUnit();
				Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
				Vector3 direction = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
				float shoulderOffsetAmount = 0.5f;
				Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * direction * shoulderOffsetAmount;
				Vector3 actionCameraPositon = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (direction * -1);
				actionCamGameObject.transform.position = actionCameraPositon;
				actionCamGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);


				Show();
				break;
		}
	}

	private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
	{
		switch (sender)
		{
			case ShootAction shootAction:
				Hide();
				break;
		}
	}

}
