using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] private Animator unitAnimator;
    private Vector3 _targetPosition;
	GridPosition gridPosition;

	private void Awake()
	{
		_targetPosition = transform.position;
	}

	private void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
	}

	private void Update()
    {

        float stoppingDistance = 0.01f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;


			float rotateSpeed = 10f;
			transform.forward = Vector3.Lerp(transform.forward, moveDirection,Time.deltaTime * rotateSpeed);

			unitAnimator.SetBool("IsWalking", true);
		}
		else
		{
			unitAnimator.SetBool("IsWalking", false);
		}

		GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		if(newGridPosition != gridPosition)
		{
			LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
			gridPosition = newGridPosition;
		}
	}

    public void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }

}
