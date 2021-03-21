using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchAction : MonoBehaviour, IPointerClickHandler
{
	public ApplicationStruct.EventRay onTouchClickRay;

	public void OnPointerClick(PointerEventData eventData)
	{
		onTouchClickRay.Invoke(Camera.main.ScreenPointToRay(eventData.position));
	}
}
