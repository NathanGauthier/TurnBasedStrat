using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
	public event EventHandler OnStartMoving;
	public event EventHandler OnStopMoving;

	[SerializeField] private int maxMoveDistance = 4;

	private List<Vector3> targetPositionList;
	private int currentPositionIndex;



	// Update is called once per frame
	void Update()
    {

		if (!isActive) return;

		Vector3 targetPosition = targetPositionList[currentPositionIndex];
		Vector3 moveDirection = (targetPosition - transform.position).normalized;

		float rotateSpeed = 10f;
		transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

		float stoppingDistance = 0.1f;

		if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
		{
			float moveSpeed = 4f;
			transform.position += moveDirection * Time.deltaTime * moveSpeed;
			
		}

		else
		{
			currentPositionIndex++;
			if(currentPositionIndex >= targetPositionList.Count)
			{
				OnStopMoving?.Invoke(this, EventArgs.Empty);
				ActionComplete();
			}
			
        }
		
		
	}

	public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
	{
		List<GridPosition> gridPositionList =  Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetPosition, out int pathLength);
		currentPositionIndex = 0;
		targetPositionList = new List<Vector3>();
		foreach(GridPosition gridPosition in gridPositionList)
		{
			targetPositionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
		}
		
		OnStartMoving?.Invoke(this, EventArgs.Empty); 

		ActionStart(onMoveComplete);
	}

	

	public override List<GridPosition> GetValidActionGridPositionList()
	{
		List<GridPosition> validGridPositionList = new List<GridPosition>();

		GridPosition unitGridPosition = unit.GetGridPosition();

		for(int x = - maxMoveDistance; x <= maxMoveDistance; x++)
		{
			for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
			{
				GridPosition offsetGridPosition = new GridPosition(x, z);
				GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

				if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					continue;
				}
				
				if (testGridPosition == unitGridPosition)
				{
					continue;
				}

				if(LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
				{
					continue;
				}
				if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
				{
					continue;
				}

				if(Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
				{
					continue;
				}

				int pathfindingDistanceMultiplier = 10; 
				if(Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
				{
					continue;
				}

				validGridPositionList.Add(testGridPosition);
				//Debug.Log(testGridPosition);
			}
		}

		return validGridPositionList;
	}

	public override string GetActionName()
	{
		return "Move";
	}

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
		return new EnemyAIAction { gridPosition = gridPosition, actionValue = targetCountAtGridPosition * 10 };
	}

}
