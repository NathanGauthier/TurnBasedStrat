using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

	private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private LayerMask UnitsLayerMask;

	private void Awake()
	{
		instance = this;
	}
	private void Update()
    {         
        transform.position = GetPosition();	
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
		return raycastHit.point;
    }

	//public static Unit GetUnit()
	//{
	//	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	//	Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.UnitsLayerMask);
	//	return raycastHit.transform.GetComponent<Unit>();
	//}

}
