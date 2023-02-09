using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    private enum State 
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State state;

    private int maxShootDistance = 7;

    private float stateTimer;

    private Unit targetUnit;

    private bool canShoot;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;
        switch (state) 
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;


            case State.Shooting:
                if(canShoot) 
                {
                    Shoot();
                    canShoot = false;
                }
                break;


            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0)
        {
            NextState();
        }
    }

    private void NextState()
    {



        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = .1f;
                stateTimer = shootingStateTime;
                break;


            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = .5f;
                stateTimer = coolOffStateTime;
                break;


            case State.Cooloff:

                ActionComplete();
        
                break;
        }
    }

    private void Shoot()
    {
        targetUnit.Damage();
    }

    public override string GetActionName()
    {
        return "Shoot";    
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                
                if(testDistance > maxShootDistance)
                {
                    continue;
                }
        
                if (!LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
                {
                    // Grid is empty
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //same team
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
                //Debug.Log(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShoot = true;

    }
}

    