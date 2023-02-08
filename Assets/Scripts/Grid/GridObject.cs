using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
	private GridSystem gridSystem;
	private GridPosition gridPosition;
	private List<Unit> units;

	public GridObject(GridSystem gridSystem, GridPosition gridPosition)
	{
		this.gridSystem = gridSystem;
		this.gridPosition = gridPosition;
		units = new List<Unit>();
	}

	public override string ToString()
	{
		string unitString = "";
		foreach (Unit unit in units)
		{
			unitString += unit + "\n";
		}
		return gridPosition.ToString() + "\n" + unitString;
	}

	public void AddUnit(Unit unit)
	{
		units.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		units.Remove(unit);
	}

	public List<Unit> GetUnitList()
	{
		return units;
	}

	public bool HasAnyUnit()
	{
		return units.Count > 0;
	}
}
