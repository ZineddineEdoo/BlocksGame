using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SceneChanger;

/// <summary>
/// Requires SceneChanger Component in Parent Hierarchy
/// </summary>
public class SceneNavigatorButton : MonoBehaviour
{
	[SerializeField]
	private Scene destinationScene = default;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() =>
		{			
			GetComponentInParent<SceneChanger>().ChangeSceneTo(destinationScene);
		});
	}
}
