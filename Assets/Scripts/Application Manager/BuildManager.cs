using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	void Awake()
	{
#if UNITY_EDITOR
		CheckVersion();
#endif

//#if UNITY_STANDALONE
//		SetStandaloneResolution();
//#endif
	}

	private void CheckVersion()
	{
#if INSTANT
		Debug.Log("Instant Version");
#else
		Debug.Log("Full Version");
#endif
	}

	private void SetStandaloneResolution()
	{
		Screen.SetResolution(720, 1280, FullScreenMode.Windowed);
	}
}
