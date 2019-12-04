using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	private const float ASPECT_RATIO = 9f / 16f;
	private const float WIDTH = 2.8125f;
	private const float HEIGHT = 5f;

	public static BuildManager Instance { get; private set; }

	private readonly Vector2 screenBounds = new Vector2(WIDTH, HEIGHT);

	public static Vector2 ScreenBounds =>
		Instance != null ? Instance.screenBounds : new Vector2(WIDTH, HEIGHT);

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
		{
			Instance = this;

			#if UNITY_EDITOR
				CheckVersion();
			#endif
		}
	}

	private void CheckVersion()
	{
#if DEMO
		Debug.Log("Demo Version");
#else
		Debug.Log("Full Version");
#endif
	}

	void Update()
	{
#if UNITY_WSA
		// To Restrict Min Window Size
		if ((float)Screen.width / Screen.height < ASPECT_RATIO)
		{
			Screen.SetResolution(216, 384, false);
		}
#endif
	}
}
