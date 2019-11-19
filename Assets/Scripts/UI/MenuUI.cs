using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SceneController;

public class MenuUI : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager = default;

	public GameManager GameManager => gameManager;

	void Awake()
	{
		gameManager.GameEnding += (s, e) =>
		{
			SceneController.Instance.ChangeSceneTo(Scene.GameOver, GetComponentInParent<AnimationEventsManager>());
		};
	}

	/// <summary>
	/// Used to unlock GameStarted Event
	/// </summary>
	public void OnUILoaded() => gameManager.IsUILoaded = true;
}
