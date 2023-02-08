using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
	private void Start()
	{
		UnitActionSystem_OnBusyChanged(this, false);
		UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
	}

	private void UnitActionSystem_OnBusyChanged(object sender,bool bIsBusy)
	{		
		gameObject.SetActive(bIsBusy);
	}

}
