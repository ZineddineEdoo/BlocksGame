using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SceneManager;

public class SceneNavigatorButton : MonoBehaviour
{
	[SerializeField]
	private Scene destinationScene = default;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() =>
		{
			if (destinationScene == Scene.Achievements)
				SceneManager.Instance.LoadAdditiveScene(destinationScene);
			else
				SceneManager.Instance.ChangeSceneTo(destinationScene, GetComponentInParent<SceneAnimationEventsManager>());
		});
	}
}
