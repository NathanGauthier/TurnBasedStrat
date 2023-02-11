using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

	private float totalSpinAmount;

	private void Update()
	{
		if (!isActive) return;

		float fSpinAddAmount = 360f * Time.deltaTime;
		transform.eulerAngles += new Vector3(0, fSpinAddAmount, 0);

		totalSpinAmount += fSpinAddAmount;

		if (totalSpinAmount >= 360f)
		{
            ActionComplete();
        }

	}
	public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
	{
        totalSpinAmount = 0f;

        ActionStart(onSpinComplete);
	}

	public override string GetActionName()
	{
		return "Spin";
	}
	public override List<GridPosition> GetValidActionGridPositionList()
	{
		GridPosition unitGridPosition = unit.GetGridPosition();

		return new List<GridPosition> { unitGridPosition };
	}

	public override int GetActionPointsCost()
	{
		return 2;
	}
}
