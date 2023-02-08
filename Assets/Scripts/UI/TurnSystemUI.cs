using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI turnNumberText;

	[SerializeField] private Button endTurnButton;

	private void Start()
	{
		endTurnButton.onClick.AddListener(() =>
		{
			TurnSystem.Instance.NextTurn();
		});

		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

		UpdateTurnNumberText();
	}

	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		UpdateTurnNumberText();
	}

	public void UpdateTurnNumberText()
	{
		turnNumberText.text = "TURN : " + TurnSystem.Instance.GetTurnNumber().ToString(); 
	}

}
