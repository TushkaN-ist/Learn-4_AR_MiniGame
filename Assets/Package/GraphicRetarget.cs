using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicRetarget : Graphic
{
	public Graphic[] graphics;
	public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB) {
		if (graphics==null)
			return;
		foreach (var item in graphics)
		{
			if (item!=null){
				item.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
			}
		}
	}
}
