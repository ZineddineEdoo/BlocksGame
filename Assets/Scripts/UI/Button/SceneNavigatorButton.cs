using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SceneController;

public class SceneNavigatorButton : MonoBehaviour
{
	[SerializeField]
	private Scene destinationScene = default;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() =>
		{
			FindObjectOfType<SceneController>().ChangeSceneTo(destinationScene, GetComponentInParent<Animator>());
		});
	}
}
