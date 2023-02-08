using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Testing : MonoBehaviour
{
	[SerializeField] private Unit unit;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{			
			GridSystemVisual.Instance.HideAllGridPosition();
			GridSystemVisual.Instance.ShowGridPositionList(	unit.GetMoveAction().GetValidActionGridPositionList());
		}
	}
}
