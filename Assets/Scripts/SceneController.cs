using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public enum Scene
	{
		Global,
		MainMenu,
		Main,
		GameOver,
		Pause,
		Achievements
	}

	public static SceneController Instance { get; private set; }

	/// <summary>
	/// Only used in Editor; Release Builds Will Load Global Scene First
	/// </summary>
	public static void LoadStart()
	{
		if (Instance == null)
			SceneManager.LoadSceneAsync((int)Scene.Global);
	}

	public static void Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public Scene CurrentScene { get; private set; }

	void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);
		else
		{
			Instance = this;

#if UNITY_STANDALONE
		Screen.SetResolution(432, 768, FullScreenMode.Windowed);
#endif
			SaveManager.Load();

			SceneManager.LoadSceneAsync((int)Scene.MainMenu);
			CurrentScene = Scene.MainMenu;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Time.timeScale == 0f)       // Paused
				ResumeGame();
			else if (CurrentScene == Scene.GameOver)
				ChangeSceneTo(Scene.Main);
			else if (CurrentScene == Scene.Achievements)
				ChangeSceneTo(Scene.MainMenu);
			else if (CurrentScene == Scene.MainMenu)
				Exit();
		}
		else if (Input.GetKeyDown(KeyCode.Menu))
		{
			if (CurrentScene == Scene.Achievements || CurrentScene == Scene.GameOver)
				ChangeSceneTo(Scene.MainMenu);
		}
	}

	void OnApplicationFocus(bool focus) => CheckPauseGame(!focus);

	void OnApplicationPause(bool pause) => CheckPauseGame(pause);

	private void CheckPauseGame(bool shouldPause)
	{
		if (!SceneManager.GetSceneByBuildIndex((int)Scene.Pause).isLoaded
			&& SceneManager.GetSceneByBuildIndex((int)Scene.Main).isLoaded
			&& shouldPause)
		{
			PauseGame();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
		SceneManager.LoadSceneAsync((int)Scene.Pause, LoadSceneMode.Additive);
	}

	public void ResumeGame()
	{
		UnloadScene((int)Scene.Pause);
		Time.timeScale = 1f;
	}

	public void ChangeSceneTo(Scene scene, Animator animator = null)
	{
		if (CurrentScene != scene)
		{
			CurrentScene = scene;

			if (animator != null)
				FadeOutScene(animator);
			else
				LoadSceneImmediately(CurrentScene);
		}
	}

	public void FadeOutScene(Animator animator) => animator.SetBool("FadeOut", true);

	public void LoadSceneImmediately() => SceneManager.LoadSceneAsync((int)CurrentScene);

	public void LoadSceneImmediately(Scene scene) => SceneManager.LoadSceneAsync((int)scene);

	private void UnloadScene(int buildIndex) => SceneManager.UnloadSceneAsync(buildIndex);
}
