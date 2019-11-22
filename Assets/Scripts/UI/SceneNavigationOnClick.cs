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
		{
			var animEventsManager = GetComponentInParent<AnimationEventsManager>();

			SceneManager.Instance.ChangeSceneTo(destinationScene, animEventsManager);
		}
#else
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);
			var animEventsManager = GetComponentInParent<AnimationEventsManager>();

			SceneManager.Instance.ChangeSceneTo(destinationScene, animEventsManager);
		}
#endif
	}
}
