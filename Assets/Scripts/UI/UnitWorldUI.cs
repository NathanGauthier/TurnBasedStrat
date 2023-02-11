using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI actionPointsText;
	[SerializeField] private Unit unit;
	[SerializeField] private Image healthBarImage;
	[SerializeField] private HealthSystem healthSystem;

	private void Start()
	{
		Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
		UpdateActionPointsText();
		UpdateHealthBar();
		healthSystem.OnDamageTook += HealthSystem_OnDamageTook;
	}

	private void UpdateActionPointsText()
	{
		actionPointsText.text = unit.GetActionPoints().ToString() ;
	}

	private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
	{
		UpdateActionPointsText();
	}

	private void UpdateHealthBar()
	{
		healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
	}

	private void HealthSystem_OnDamageTook(object sender, EventArgs e)
	{
		UpdateHealthBar();
	}
}
