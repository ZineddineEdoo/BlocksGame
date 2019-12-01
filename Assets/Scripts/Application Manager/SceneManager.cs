using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
	public static SceneManager Instance { get; private set; }

	public static void Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public enum Scene
	{
		Global,
		MainMenu,
		Tutorial,
		Main,
		GameOver,
		Pause,
		Achievements
	}

	public Scene CurrentScene { get; private set; }

	private bool isPaused;

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
		{
			Instance = this;
			SaveManager.Load();

			UnitySceneManager.LoadSceneAsync((int)Scene.MainMenu);
			CurrentScene = Scene.MainMenu;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
				ResumeGame();
			else if (CurrentScene == Scene.GameOver)
				ChangeSceneTo(Scene.Main);
			else if (CurrentScene == Scene.Achievements || CurrentScene == Scene.Tutorial)
				ChangeSceneTo(Scene.MainMenu);
			else if (CurrentScene == Scene.MainMenu)
				Exit();
		}
		else if (Input.GetKeyDown(KeyCode.Menu))
		{
			if (CurrentScene == Scene.Achievements || CurrentScene == Scene.GameOver || CurrentScene == Scene.Tutorial)
				ChangeSceneTo(Scene.MainMenu);
		}
	}

	/// <summary>
	/// If Focus is False, Game will be Paused
	/// </summary>
	/// <param name="focus"></param>
	void OnApplicationFocus(bool focus) => CheckPauseGame(!focus);

	/// <summary>
	/// If Pause is True, Game will be Paused
	/// </summary>
	/// <param name="pause"></param>
	void OnApplicationPause(bool pause) => CheckPauseGame(pause);

	private void CheckPauseGame(bool shouldPause)
	{
		if (!isPaused && CurrentScene == Scene.Main	&& shouldPause)
			PauseGame();
	}

	public void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0f;
		UnitySceneManager.LoadSceneAsync((int)Scene.Pause, LoadSceneMode.Additive);
	}

	public void ResumeGame(SceneAnimationEventsManager animEventsManager = null) => 
		this.RestartCoroutine(AnimateResumeGame(animEventsManager), nameof(AnimateResumeGame));

	private IEnumerator AnimateResumeGame(SceneAnimationEventsManager animEventsManager)
	{
		if (animEventsManager == null)
			animEventsManager = FindObjectsOfType<SceneAnimationEventsManager>()
				.FirstOrDefault(m => m.gameObject.scene == UnitySceneManager.GetSceneByBuildIndex((int)Scene.Pause));
		
		// Found
		if (animEventsManager != null)
		{
			animEventsManager.FadeOut();

			yield return new WaitUntil(() => animEventsManager.FadedOut);
		}

		UnloadScene((int)Scene.Pause);
		isPaused = false;
		Time.timeScale = 1f;

		this.RemoveCoroutine(nameof(AnimateResumeGame));
	}

	public void ChangeSceneTo(Scene scene, SceneAnimationEventsManager animEventsManager = null) =>
		this.RestartCoroutine(AnimateChangeSceneTo(scene, animEventsManager), nameof(AnimateChangeSceneTo));

	private IEnumerator AnimateChangeSceneTo(Scene scene, SceneAnimationEventsManager animEventsManager)
	{
		if (CurrentScene != scene)
		{
			if (animEventsManager == null)
				animEventsManager = FindObjectOfType<SceneAnimationEventsManager>();

			CurrentScene = scene;

			// Found
			if (animEventsManager != null)
			{
				animEventsManager.FadeOut();

				yield return new WaitUntil(() => animEventsManager.FadedOut);
			}

			LoadSceneImmediately();

			this.RemoveCoroutine(nameof(AnimateChangeSceneTo));
		}
	}

	/// <summary>
	/// Loads Current Scene
	/// </summary>
	public void LoadSceneImmediately() => UnitySceneManager.LoadSceneAsync((int)CurrentScene);

	private void UnloadScene(int buildIndex) => UnitySceneManager.UnloadSceneAsync(buildIndex);
}
