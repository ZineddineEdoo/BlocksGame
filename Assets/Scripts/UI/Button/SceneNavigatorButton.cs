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
			SceneManager.Instance.ChangeSceneTo(destinationScene, GetComponentInParent<SceneAnimationEventsManager>());
		});
	}
}
