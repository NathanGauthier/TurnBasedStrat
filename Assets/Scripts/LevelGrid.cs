using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
	public static LevelGrid Instance { get; private set; }

	[SerializeField] private Transform gridDebugObjectPrefab;

	private GridSystem gridSystem;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There's more than one LevelGrid" + transform + " - " + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;

		gridSystem = new GridSystem(10, 10, 2f);
		gridSystem.CreateDebugObject(gridDebugObjectPrefab);
	}

	public void AddUnitAtGridPosition(GridPosition gridPos, Unit unit)
	{
		gridSystem.GetGridObject(gridPos).AddUnit(unit);
	}

	public List<Unit> GetUnitListAtGridPosition(GridPosition gridPos)
	{
		return gridSystem.GetGridObject(gridPos).GetUnitList();
	}

	public void RemoveUnitAtGridPosition(GridPosition gridPos,Unit unit)
	{
		gridSystem.GetGridObject(gridPos).RemoveUnit(unit);
	}

	public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPos, GridPosition toGridPos)
	{
		RemoveUnitAtGridPosition(fromGridPos,unit);
		AddUnitAtGridPosition(toGridPos, unit);
	}

	public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

}

	
