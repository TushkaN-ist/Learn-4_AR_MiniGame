using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ApplicationStruct {
	
	[System.Serializable]
	public class EventFloat : UnityEvent<float>{}
	[System.Serializable]
	public class EventInt : UnityEvent<int>{}
	[System.Serializable]
	public class EventLong : UnityEvent<long>{}
	[System.Serializable]
	public class EventString : UnityEvent<string>{}
	[System.Serializable]
	public class EventColor : UnityEvent<Color>{}
	[System.Serializable]
	public class EventBool : UnityEvent<bool>{}
	[System.Serializable]
	public class EventOrientation : UnityEvent<DeviceOrientation> { }
	[System.Serializable]
	public class EventVector2 : UnityEvent<Vector2> { }
	[System.Serializable]
	public class EventVector3 : UnityEvent<Vector3> { }
	[System.Serializable]
	public class EventVector4 : UnityEvent<Vector4> { }
	[System.Serializable]
	public class EventRay : UnityEvent<Ray> { }
}
