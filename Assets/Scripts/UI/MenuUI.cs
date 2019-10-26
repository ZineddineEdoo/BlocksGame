using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Requires SceneChanger Component on the Same GameObject
/// </summary>
public class MenuUI : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager = default;

	public GameManager GameManager => gameManager;

	void Awake()
	{
		gameManager.GameEnding += (s, e) => GetComponent<SceneChanger>().ChangeSceneTo(SceneChanger.GAME_OVER_SCENE);
	}

	/// <summary>
	/// Used to unlock GameStarted Event
	/// </summary>
	public void OnUILoaded() => gameManager.IsUILoaded = true;
}
