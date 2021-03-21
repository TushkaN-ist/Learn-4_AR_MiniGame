using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ApplicationCore : MonoBehaviour
{
	
	static string _platform=null;
	
	public static string get_platform{
		get {
			if (string.IsNullOrEmpty(_platform)){
				switch (Application.platform)
				{
#if UNITY_EDITOR
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.LinuxEditor:
				case RuntimePlatform.OSXEditor:
					return UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString();
#endif
				case RuntimePlatform.WindowsPlayer:
					return _platform = "Windows";
				case RuntimePlatform.LinuxPlayer:
					return _platform = "Linux";
				case RuntimePlatform.OSXPlayer:
					return _platform = "MacOS";
				case RuntimePlatform.Android:
					return _platform = "Android";
				case RuntimePlatform.IPhonePlayer:
					return _platform = "iOS";
				default:
					return _platform = "Unknown";
				}
			}
			else
				return _platform;
		}
	}
	
	#if UNITY_EDITOR
	[SerializeField]
	FrameRate frameRate=FrameRate.FPS60;
	#endif
	public int fps=60;
	[SerializeField]
	bool neverSleep=false;

	public ApplicationStruct.EventString onAplicationActived;

	public void SetFrameRate(FrameRate fRate,int _frameRate=60){
		switch (fRate)
		{
		case FrameRate.FPS24:
			SetFrameRate(24);
			break;
		case FrameRate.FPS30:
			SetFrameRate(30);
			break;
		case FrameRate.FPS60:
			SetFrameRate(60);
			break;
		case FrameRate.FPS90:
			SetFrameRate(90);
			break;
		case FrameRate.FPS120:
			SetFrameRate(120);
			break;
		case FrameRate.FPSCustom:
			SetFrameRate(_frameRate);
			return;
		}
	}
	public void SetFrameRate(int _frameRate){
		Application.targetFrameRate = fps = _frameRate;
	}
	// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
	protected void OnValidate() {
	#if UNITY_EDITOR
		SetFrameRate(frameRate,fps);
	#else
		SetFrameRate(fps);
	#endif
	}
	public void OpenURL(string url){
		Application.OpenURL(url);
	}
	public void CopyText(string str){
		GUIUtility.systemCopyBuffer = str;
	}
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		OnValidate();
		if (neverSleep)
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		onAplicationActived.Invoke(get_platform);
	}
	public void ChangeScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void SetOrientation(int orientation)
	{
		Screen.orientation = (ScreenOrientation)orientation;
	}
	public void ApplicationQuit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

#if UNITY_EDITOR
	[MenuItem("Tools/Clear PlayerPrefs")]
	private static void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
	[MenuItem("Tools/Clear Cache")]
	public static void CleanCache ()
	{
		if (Caching.ClearCache ()) 
		{
			Debug.Log("Successfully cleaned the cache.");
		}
		else 
		{
			Debug.Log("Cache is being used.");
		}
	} 
#endif
	public static void ForceRebuildLayout(RectTransform rectTransform)
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
	}
	
	public enum FrameRate{
		FPSCustom,
		FPS24,
		FPS30,
		FPS60,
		FPS90,
		FPS120
	}
}
