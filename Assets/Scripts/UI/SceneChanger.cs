using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public const int MAIN_MENU_SCENE = 0;
	public const int MAIN_SCENE = 1;
	public const int GAME_OVER_SCENE = 2;
	public const int PAUSE_SCENE = 3;
	public const int ACHIEVEMENTS_SCENE = 4;

	public enum Scene
	{
		MainMenu = MAIN_MENU_SCENE,
		Main = MAIN_SCENE,
		GameOver = GAME_OVER_SCENE,
		Pause = PAUSE_SCENE,
		Achievements = ACHIEVEMENTS_SCENE
	}

	private int newIndex;

	void Awake()
	{
#if UNITY_STANDALONE
		Screen.SetResolution(432, 768, FullScreenMode.Windowed);
#endif
	}

	void OnApplicationFocus(bool focus)
	{
		CheckPauseGame(!focus);
	}

	void OnApplicationPause(bool pause)
	{
		CheckPauseGame(pause);
	}

	private void CheckPauseGame(bool shouldPause)
	{
		if (!SceneManager.GetSceneByBuildIndex(PAUSE_SCENE).isLoaded
			&& SceneManager.GetSceneByBuildIndex(MAIN_SCENE).isLoaded
			&& shouldPause)
		{
			PauseGame();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
		SceneManager.LoadSceneAsync(PAUSE_SCENE, LoadSceneMode.Additive);
	}

	/// <summary>
	/// Used by Animation Event (When Pause Menu has Faded Out)
	/// </summary>
	public void ResumeGame()
	{
		UnloadScene(PAUSE_SCENE);
		Time.timeScale = 1f;
	}

	public void ChangeSceneTo(Scene scene)
	{
		switch (scene)
		{
			case Scene.MainMenu:
			default:
				ChangeSceneTo(MAIN_MENU_SCENE);
				break;
			case Scene.Main:
				ChangeSceneTo(MAIN_SCENE);
				break;
			case Scene.GameOver:
				ChangeSceneTo(GAME_OVER_SCENE);
				break;
			case Scene.Pause:
				ChangeSceneTo(PAUSE_SCENE);
				break;
			case Scene.Achievements:
				ChangeSceneTo(ACHIEVEMENTS_SCENE);
				break;
		}
	}

	public void ChangeSceneTo(int buildIndex)
	{
		newIndex = buildIndex;

		if (GetComponent<Animator>() != null)
			FadeOutScene();
		else
			OnAnimationComplete();
	}

	public void InsertScene(int buildIndex)
	{
		newIndex = buildIndex;

		SceneManager.LoadSceneAsync(newIndex, LoadSceneMode.Additive);
	}

	/// <summary>
	/// Used by Resume Button
	/// </summary>
	public void FadeOutScene()
	{
		GetComponent<Animator>().SetBool("FadeOut", true);
	}

	/// <summary>
	/// Used by Animation Events to indicate that an Animation is Complete. <br/>
	/// SceneManager then Loads the Scene Previously Requested.
	/// </summary>
	public void OnAnimationComplete() => SceneManager.LoadSceneAsync(newIndex);

	private void UnloadScene(int buildIndex) => SceneManager.UnloadSceneAsync(buildIndex);
}
