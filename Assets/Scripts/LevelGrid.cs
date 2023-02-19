using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
	public static LevelGrid Instance { get; private set; }


	public event EventHandler OnAnyUnitMovedGridPosition;


	[SerializeField] private Transform gridDebugObjectPrefab;

	[SerializeField] private int height;
	[SerializeField] private int width;
	[SerializeField] private float cellSize;

	private GridSystem<GridObject> gridSystem;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There's more than one LevelGrid" + transform + " - " + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;

		gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
	}

	private void Start()
	{
		Pathfinding.Instance.Setup(width, height, cellSize);
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

		OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
	}

	public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
	public Vector3 GetWorldPosition(GridPosition gridPos) => gridSystem.GetWorldPosition(gridPos);
	public bool IsValidGridPosition(GridPosition gridPos) => gridSystem.IsValidGridPosition(gridPos);
	public int GetWidth() => gridSystem.GetWidth();
	public int GetHeight() => gridSystem.GetHeight();

	public bool HasUnitOnGridPosition(GridPosition gridPos)
	{
		return gridSystem.GetGridObject(gridPos).HasAnyUnit();
	}
    public Unit GetUnitAtGridPosition(GridPosition gridPos)
    {
        return gridSystem.GetGridObject(gridPos).GetUnit();
    }


}


