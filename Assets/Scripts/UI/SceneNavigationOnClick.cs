using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SceneManager;

public class SceneNavigationOnClick : MonoBehaviour
{
	[SerializeField]
	private Scene destinationScene = default;

	void Update()
	{
#if UNITY_EDITOR || UNITY_WSA
		GetPCInput();
#else
		GetMobileInput();
#endif
	}
	
	private void GetMobileInput()
	{
		if (Input.touchCount > 0)
			ChangeScene();
	}

	private void GetPCInput()
	{
		if (Input.GetMouseButton(0))
			ChangeScene();
	}

	private void ChangeScene()
	{
		SceneManager.Instance.ChangeSceneTo(destinationScene, GetComponentInParent<SceneAnimationEventsManager>());
		this.enabled = false;
	}
}
