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
	private Stack<Scene> additiveScenes;

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
		{
			Instance = this;
			additiveScenes = new Stack<Scene>();

			SaveManager.Load();

			UnitySceneManager.LoadSceneAsync((int)Scene.MainMenu);
			CurrentScene = Scene.MainMenu;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (additiveScenes.Count > 0 && additiveScenes.Peek() != Scene.Pause)
				UnloadLastAdditiveScene();
			else if (isPaused)
				ResumeGame();
			else if (CurrentScene == Scene.GameOver)
				ChangeSceneTo(Scene.Main);
			else if (CurrentScene == Scene.Tutorial)
				ChangeSceneTo(Scene.MainMenu);
			else if (CurrentScene == Scene.MainMenu)
				Exit();
		}
		else if (Input.GetKeyDown(KeyCode.Menu))
		{
			if (CurrentScene == Scene.GameOver || CurrentScene == Scene.Tutorial)
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
		LoadAdditiveScene(Scene.Pause);
	}

	public void ResumeGame(SceneAnimationEventsManager animEventsManager = null)
	{
		UnloadLastAdditiveScene(animEventsManager, () =>
		{
			isPaused = false;
			Time.timeScale = 1f;
		});
	}

	/// <summary>
	/// Loads scene only if not already loaded
	/// </summary>
	/// <param name="scene"></param>
	public void LoadAdditiveScene(Scene scene)
	{
		if (!additiveScenes.Contains(scene))
		{
			UnitySceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);

			additiveScenes.Push(scene);
		}
	}

	public void UnloadLastAdditiveScene(SceneAnimationEventsManager animEventsManager = null, Action onUnloaded = null) =>
		this.RestartCoroutine(AnimateUnloadAdditiveScene(animEventsManager, onUnloaded), nameof(AnimateUnloadAdditiveScene));

	private IEnumerator AnimateUnloadAdditiveScene(SceneAnimationEventsManager animEventsManager, Action onUnloaded = null)
	{
		if (additiveScenes.Count > 0)
		{
			if (animEventsManager == null)
				animEventsManager = FindObjectsOfType<SceneAnimationEventsManager>()
					.FirstOrDefault(m => m.gameObject.scene == UnitySceneManager.GetSceneByBuildIndex((int)additiveScenes.Peek()));

			// Found
			if (animEventsManager != null)
			{
				animEventsManager.FadeOut();

				yield return new WaitUntil(() => animEventsManager.FadedOut);
			}

			var scene = additiveScenes.Pop();

			UnloadScene((int)scene);
			onUnloaded?.Invoke();
		}

		this.RemoveCoroutine(nameof(AnimateUnloadAdditiveScene));
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
