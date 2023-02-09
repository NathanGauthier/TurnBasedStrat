using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
	[SerializeField] private Animator animator;

	private void Awake()
	{
		if(TryGetComponent<MoveAction>(out MoveAction MoveAction))
		{
			MoveAction.OnStartMoving += MoveAction_OnStartMoving;
			MoveAction.OnStopMoving += MoveAction_OnStopMoving;
		}

		if(TryGetComponent<ShootAction>(out ShootAction ShootAction))
		{
			ShootAction.OnShoot += ShootAction_OnShoot;
		}
	}

	private void MoveAction_OnStartMoving(object sender, EventArgs e)
	{
		animator.SetBool("IsWalking", true);
	}
	
	private void MoveAction_OnStopMoving(object sender, EventArgs e)
	{
		animator.SetBool("IsWalking", false);
	}

	private void ShootAction_OnShoot(object sender, EventArgs e)
	{
		animator.SetTrigger("Shoot");
	}
}
