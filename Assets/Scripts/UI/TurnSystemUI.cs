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
	[SerializeField] private GameObject enemyTurnVisualGameObject;

	private void Start()
	{
		endTurnButton.onClick.AddListener(() =>
		{
			TurnSystem.Instance.NextTurn();
		});

		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

		UpdateTurnNumberText();
    	UpdateEnemyTurnVisual();
		UpdateEndTurnButtonVisibilty();
    }

	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		UpdateTurnNumberText();
		UpdateEnemyTurnVisual();
		UpdateEndTurnButtonVisibilty();
    }

	public void UpdateTurnNumberText()
	{
		turnNumberText.text = "TURN : " + TurnSystem.Instance.GetTurnNumber().ToString(); 
	}

	private void UpdateEnemyTurnVisual()
	{
		enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
	}

	private void UpdateEndTurnButtonVisibilty()
	{
		endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
	}

}
