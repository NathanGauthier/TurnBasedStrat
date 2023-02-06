using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
	private int nWidth;
	private int nHeight;
	private float fCellSize;
	private GridObject[,] gridObjectArray;
    public GridSystem(int width, int height, float cellSize) 
	{ 
		this.nWidth = width;
		this.nHeight = height;
		this.fCellSize = cellSize;

		gridObjectArray = new GridObject[nWidth,nHeight];

		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				GridPosition gridPos = new GridPosition(x, z);
				gridObjectArray[x,z] =  new GridObject(this, gridPos);
			}
		}
	}

	public Vector3 GetWorldPos(GridPosition gridPos)
	{
		return new Vector3(gridPos.x, 0, gridPos.z) * fCellSize;
	}

	public GridPosition GetGridPosition(Vector3 worldPos)
	{
		return new GridPosition(
			Mathf.RoundToInt(worldPos.x / fCellSize),
			Mathf.RoundToInt(worldPos.z / fCellSize)
			);
	}

	public void CreateDebugObject(Transform debugPrefab)
	{
		for (int x = 0; x < nWidth; x++)
		{
			for (int z = 0; z < nHeight; z++)
			{
				GridPosition gridPos = new GridPosition(x, z);
				Transform debugTransform  = GameObject.Instantiate(debugPrefab, GetWorldPos(gridPos), Quaternion.identity);
				GridDebugObject gridDebugObject =  debugTransform.GetComponent<GridDebugObject>();
				gridDebugObject.SetGridObject(GetGridObject(gridPos));
			}
		}
	}

	public GridObject GetGridObject(GridPosition gridPos)
	{
		return gridObjectArray[gridPos.x, gridPos.z];
	}
}
