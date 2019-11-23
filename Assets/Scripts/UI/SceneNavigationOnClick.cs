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
#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetMouseButton(0))
#else
		if (Input.touchCount > 0)
#endif
		{
			ChangeScene();
		}
	}

	private void ChangeScene()
	{
		SceneManager.Instance.ChangeSceneTo(destinationScene, GetComponentInParent<SceneAnimationEventsManager>());
		this.enabled = false;
	}
}
