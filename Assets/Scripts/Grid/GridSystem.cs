using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{ 
	private int width;
	private int height;
	private float cellSize;
	private TGridObject[,] gridObjectArray;
    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>,GridPosition, TGridObject> createGridObject) 
	{ 
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;

		gridObjectArray = new TGridObject[this.width, this.height];

		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				GridPosition gridPos = new GridPosition(x, z);
				gridObjectArray[x,z] = createGridObject(this, gridPos);
			}
		}
	}

	public Vector3 GetWorldPosition(GridPosition gridPos)
	{
		return new Vector3(gridPos.x, 0, gridPos.z) * cellSize;
	}

	public GridPosition GetGridPosition(Vector3 worldPos)
	{
		return new GridPosition(
			Mathf.RoundToInt(worldPos.x / cellSize),
			Mathf.RoundToInt(worldPos.z / cellSize)
			);
	}

	public void CreateDebugObject(Transform debugPrefab)
	{
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				GridPosition gridPos = new GridPosition(x, z);
				Transform debugTransform  = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPos), Quaternion.identity);
				GridDebugObject gridDebugObject =  debugTransform.GetComponent<GridDebugObject>();
				gridDebugObject.SetGridObject(GetGridObject(gridPos));
			}
		}
	}

	public TGridObject GetGridObject(GridPosition gridPos)
	{
		return gridObjectArray[gridPos.x, gridPos.z];
	}

	public bool IsValidGridPosition(GridPosition gridPos)
	{
		return gridPos.x >= 0 && gridPos.z >= 0 && gridPos.x < width && gridPos.z < height;
	}
	public int GetWidth()
	{
		return width;
	}

	public int GetHeight()
	{
		return height;
	}
}
