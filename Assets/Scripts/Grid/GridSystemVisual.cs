using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class GridSystemVisual : MonoBehaviour
{

	public static GridSystemVisual Instance { get; private set; }

	[SerializeField] private Transform gridSystemVisualSinglePrefab;

	private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There's more than one GridSystemVisual" + transform + " - " + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetHeight(), LevelGrid.Instance.GetHeight()];
		for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
		{
			for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
			{
				GridPosition pos = new GridPosition(x, z);
				Transform gridSystemVisualSingleTransform =  Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(pos), Quaternion.identity);

				gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
			}
		}
	}

	private void Update()
	{
		UpdateGridVisual();
	}

	public void HideAllGridPosition()
	{
		foreach(GridSystemVisualSingle gridSystemVisualSingle in gridSystemVisualSingleArray)
		{
			gridSystemVisualSingle.Hide();
		}
	}

	public void ShowGridPositionList(List<GridPosition> gridPositionList)
	{
		foreach(GridPosition gridPosition in gridPositionList)
		{
			gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
		}
	}

	private void UpdateGridVisual()
	{
		HideAllGridPosition();

		BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

		ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
	}
}
