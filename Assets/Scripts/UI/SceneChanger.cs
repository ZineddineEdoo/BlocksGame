using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO Rename
public class SceneChanger : MonoBehaviour
{
	void Awake()
	{
#if UNITY_EDITOR
		// Comment To Debug Scene Without Loading Global Scene
		if (SceneController.Instance == null)
			SceneController.LoadStart();
#endif
	}

	/// <summary>
	/// Used by Animation Event
	/// </summary>
	public void OnPauseMenuFadedOut() => SceneController.Instance.ResumeGame();

	/// <summary>
	/// Used by Animation Event
	/// </summary>
	public void OnSceneFadedOut() => SceneController.Instance.LoadSceneImmediately();
}
