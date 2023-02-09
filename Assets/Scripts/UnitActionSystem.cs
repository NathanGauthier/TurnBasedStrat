using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
	public static UnitActionSystem Instance { get; private set; }

	public event EventHandler OnSelectedUnitChanged;
	public event EventHandler OnSelectedActionChanged;
	public event EventHandler<bool> OnBusyChanged;
	public event EventHandler OnActionStarted;

	[SerializeField] private Unit selectedUnit;
	[SerializeField] private LayerMask unitsLayerMask;

	private BaseAction selectedAction;

	private bool _bIsBusy;

	private void Awake()
	{
		if(Instance != null)
		{
			Debug.LogError("There's more than one UnitActionSystem" + transform + " - " + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		SetSelectedUnit(selectedUnit);
	}

	private void Update()
	{
		if (_bIsBusy) return;

		if (!TurnSystem.Instance.IsPlayerTurn()) return;

		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (TryHandleUnitsSelection()) return;
		
		HandleSelectedAction();
	}

	private void HandleSelectedAction()
	{
		if(Input.GetMouseButtonDown(0))
		{
			GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
			if(selectedAction.IsValidActionGriPosition(mouseGridPosition))
			{
				if(selectedUnit.TrySpendPointsToTakeAction(selectedAction))
				{
					SetBusy();
					selectedAction.TakeAction(mouseGridPosition, ClearBusy);
					OnActionStarted?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}

	private void SetBusy()
	{
		_bIsBusy = true;
		OnBusyChanged?.Invoke(this, _bIsBusy);
	}

	private void ClearBusy()
	{
		_bIsBusy = false;
		OnBusyChanged?.Invoke(this, _bIsBusy);
	}

	private bool TryHandleUnitsSelection()
	{
		if (Input.GetMouseButtonDown(0))
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
			{
				if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
				{
					if(unit == selectedUnit)
					{
						return false;
					}

					if(unit.IsEnemy())
					{
						return false;
					}
					SetSelectedUnit(unit);
					return true;
				}
			}
		}
		return false;	
	}

	private void SetSelectedUnit(Unit unit)
	{
		selectedUnit = unit;
		SetSelectedAction(selectedUnit.GetMoveAction());
		OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
	}

	public void SetSelectedAction(BaseAction baseAction)
	{
		selectedAction = baseAction;

		OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
	}

	public Unit GetSelectedUnit()
	{
		return selectedUnit;
	}

	public BaseAction GetSelectedAction()
	{
		return selectedAction;
	}
}
