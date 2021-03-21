using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasOrientation : UIBehaviour
{
	public void ForceOrientation(int side){
		
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		switch (side)
		{
		case 0:
			Screen.orientation = ScreenOrientation.Portrait;
			cScaler.matchWidthOrHeight=1;
			break;
		case 2:
			Screen.orientation = ScreenOrientation.PortraitUpsideDown;
			cScaler.matchWidthOrHeight=1;
			break;
		case 1:
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			cScaler.matchWidthOrHeight=0;
			break;
		case 3:
			Screen.orientation = ScreenOrientation.LandscapeRight;
			cScaler.matchWidthOrHeight=0;
			break;
		default:
			break;
		}
	}
	public void FreeOrientation(){
		Screen.autorotateToLandscapeLeft = autoL;
		Screen.autorotateToLandscapeRight = autoR;
		Screen.autorotateToPortrait = autoU;
		Screen.autorotateToPortraitUpsideDown = autoD;
	}
	bool autoL,autoR,autoU,autoD;
	CanvasScaler cScaler;
	DeviceOrientation orientation = DeviceOrientation.Unknown;
	public ApplicationStruct.EventOrientation onChangeOrientation;
	public bool dirty=true;
    // Start is called before the first frame update
	void Awake()
    {
	    OnValidate();
    }
	// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
	protected void OnValidate()
	{
		autoL = Screen.autorotateToLandscapeLeft;
		autoR = Screen.autorotateToLandscapeRight;
		autoU = Screen.autorotateToPortrait;
		autoD = Screen.autorotateToPortraitUpsideDown;
		if (cScaler==null)
		{
			cScaler=GetComponent<CanvasScaler>();
		}
		dirty=true;
	}
    // Update is called once per frame
    void Update()
	{
		if (dirty && orientation!=Input.deviceOrientation){
			bool isDirty=false;
			switch (Input.deviceOrientation)
			{
			case DeviceOrientation.Portrait:
			case DeviceOrientation.PortraitUpsideDown:
				if (Screen.height>Screen.width){
					cScaler.matchWidthOrHeight=1;
					isDirty=true;
				}
				break;
			case DeviceOrientation.LandscapeLeft:
			case DeviceOrientation.LandscapeRight:
				if (Screen.width>Screen.height){
					cScaler.matchWidthOrHeight=0;
					isDirty=true;
				}
				break;
			default:
				break;
			}
			if (isDirty){
				orientation=Input.deviceOrientation;
				onChangeOrientation.Invoke(orientation);
				dirty=false;
			}
		}
    }
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		dirty=true;
	}
	public void DirtyForce(){
		dirty=true;
	}
}
