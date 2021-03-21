using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SetCanvasBounds : MonoBehaviour {

	private static Rect GetSafeArea()
	{
		return Screen.safeArea;
	}
	Rect lastSafeArea = new Rect(0, 0, 0, 0);
	
	void ApplySafeArea(Rect area)
	{
		RectTransform panel=(RectTransform)transform;
		var anchorMin = area.position;
		var anchorMax = area.position + area.size;
		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;
		panel.anchorMin = anchorMin;
		panel.anchorMax = anchorMax;

		lastSafeArea = area;
	}
	void OnRectTransformDimensionsChange(){
		ReCalc();
		//Debug.Log("Dimensions Change");
	}
	// Sent to all game objects when the player gets or loses focus.
	protected void OnApplicationFocus(bool focus)
	{
		if (focus)
			ReCalc();
	}
	// Sent to all game objects when the player pauses.
	protected void OnApplicationPause(bool pause) {
		if (!pause)
			ReCalc();
	}
	// Update is called once per frame
	private void Awake()
	{
		ReCalc();
	}
	void ReCalc(){
		
		var safeArea = GetSafeArea();

		if (safeArea != lastSafeArea)
		{
			ApplySafeArea(safeArea);
		}
	}
}
