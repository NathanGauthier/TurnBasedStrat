using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

	[SerializeField] private Transform actionButtonPrefab;
	[SerializeField] private Transform actionButtonContainerTransform;
	[SerializeField] private TextMeshProUGUI actionPointsText;

	private List<ActionButtonUI> actionButtonUIList;

	private void Awake()
	{
		actionButtonUIList = new List<ActionButtonUI>();
	}

	private void Start()
	{
		UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
		UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
		UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
		Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

		CreateUnitActionsButtons();
		UpdateSelectedVisual();
		UpdateActionPoints();
	}

	private void CreateUnitActionsButtons()
	{
		foreach(Transform buttonTransform in actionButtonContainerTransform)
		{
			Destroy(buttonTransform.gameObject);
		}
		actionButtonUIList.Clear();

		Unit selectedUnit =  UnitActionSystem.Instance.GetSelectedUnit();

		foreach (BaseAction baseAction in  selectedUnit.GetBaseActionArray())
		{
			Transform actionButton = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
			ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
			actionButtonUI.SetBaseAction(baseAction);

			actionButtonUIList.Add(actionButtonUI);
		}
	}

	private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
	{
		CreateUnitActionsButtons();
		UpdateSelectedVisual();
		UpdateActionPoints();
	}

	private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
	{
		UpdateSelectedVisual();
	}

	private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
	{
		UpdateActionPoints();
	}

	private void UpdateSelectedVisual()
	{
		foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
		{
			actionButtonUI.UpdateSelectedVisual();
		}
	}

	private void UpdateActionPoints()
	{
		Unit selectedUnit =  UnitActionSystem.Instance.GetSelectedUnit();
		actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints().ToString();
	}

	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		UpdateActionPoints();
	}

	private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
	{
		UpdateActionPoints();
	}
}
