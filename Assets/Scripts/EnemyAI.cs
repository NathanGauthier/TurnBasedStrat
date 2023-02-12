using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	private enum State
	{
		WaitingForEnemyTurn,
		TakingTurn,
		Busy
	}

	private State state;

    private float timer = 0f;

	private void Awake()
	{
		state = State.WaitingForEnemyTurn;
	}

	private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }


    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

		switch (state)
		{
			case State.WaitingForEnemyTurn:
				break;

			case State.TakingTurn:

				timer -= Time.deltaTime;

				if (timer <= 0)
				{
					if(TryTakeEnemyAIAction(SetStateTakingTurn))
					{
						state = State.Busy;
					}
					else
					{
						TurnSystem.Instance.NextTurn();
					}
				}
				break;

			case State.Busy:
				break;
		} 
    }

	private void SetStateTakingTurn()
	{
		timer = 0.5f;
		state = State.TakingTurn;
	}

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
		if(!TurnSystem.Instance.IsPlayerTurn())
		{
			state = State.TakingTurn;
			timer = 2f;
		}
    }

	private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
	{
		foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
		{
			if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
			{
				return true;
			}
		}

		return false;
	}

	private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
	{
		EnemyAIAction bestAction = null;
		BaseAction bestBaseAction = null;
		foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
		{
			if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
			{
				continue;
			}

			if(bestAction == null)
			{
				bestAction = baseAction.GetBestEnemyAiAction();
				bestBaseAction = baseAction;
			}
			else
			{
				EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAiAction();
				if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestAction.actionValue)
				{
					bestAction = testEnemyAIAction;
					bestBaseAction = baseAction;
				}
			}
		}

		if(bestAction != null && enemyUnit.TrySpendPointsToTakeAction(bestBaseAction))
		{
			bestBaseAction.TakeAction(bestAction.gridPosition, onEnemyAIActionComplete);
			return true;
		}
		else
		{
			return false;
		}
	}
}
