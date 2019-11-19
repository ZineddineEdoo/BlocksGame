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

#if UNITY_EDITOR
	/// <summary>
	/// Release Builds Will Load Global Scene First
	/// </summary>
	public static void LoadStart()
	{
		if (Instance == null)
			UnitySceneManager.LoadSceneAsync((int)Scene.Global);
	}
#endif

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
		Main,
		GameOver,
		Pause,
		Achievements
	}

	public Scene CurrentScene { get; private set; }

	private Coroutine changeSceneCoroutine;
	private Coroutine resumeCoroutine;

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
		{
			Instance = this;

#if UNITY_STANDALONE
		Screen.SetResolution(432, 768, FullScreenMode.Windowed);
#endif
			SaveManager.Load();

			UnitySceneManager.LoadSceneAsync((int)Scene.MainMenu);
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
		if (!UnitySceneManager.GetSceneByBuildIndex((int)Scene.Pause).isLoaded
			&& UnitySceneManager.GetSceneByBuildIndex((int)Scene.Main).isLoaded
			&& shouldPause)
		{
			PauseGame();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
		UnitySceneManager.LoadSceneAsync((int)Scene.Pause, LoadSceneMode.Additive);
	}

	public void ResumeGame(AnimationEventsManager animEventsManager = null)
	{
		if (animEventsManager == null)
			animEventsManager = FindObjectsOfType<AnimationEventsManager>().FirstOrDefault(m => m.gameObject.scene == UnitySceneManager.GetSceneByBuildIndex((int)Scene.Pause));

		if (resumeCoroutine != null)
			StopCoroutine(resumeCoroutine);

		resumeCoroutine = StartCoroutine(AnimateResumeGame(animEventsManager));
	}

	private IEnumerator AnimateResumeGame(AnimationEventsManager animEventsManager)
	{
		if (animEventsManager != null)
		{
			animEventsManager.FadeOut();

			yield return new WaitUntil(() => animEventsManager.FadedOut);
		}

		UnloadScene((int)Scene.Pause);
		Time.timeScale = 1f;

		resumeCoroutine = null;
	}

	public void ChangeSceneTo(Scene scene, AnimationEventsManager animEventsManager = null)
	{
		if (animEventsManager == null)
			animEventsManager = FindObjectOfType<AnimationEventsManager>();

		if (changeSceneCoroutine != null)
			StopCoroutine(changeSceneCoroutine);

		changeSceneCoroutine = StartCoroutine(AnimateChangeSceneTo(scene, animEventsManager));
	}

	private IEnumerator AnimateChangeSceneTo(Scene scene, AnimationEventsManager animEventsManager)
	{
		if (CurrentScene != scene)
		{
			CurrentScene = scene;

			if (animEventsManager != null)
			{
				animEventsManager.FadeOut();

				yield return new WaitUntil(() => animEventsManager.FadedOut);
			}

			LoadSceneImmediately();

			changeSceneCoroutine = null;
		}
	}

	/// <summary>
	/// Loads Current Scene
	/// </summary>
	public void LoadSceneImmediately() => UnitySceneManager.LoadSceneAsync((int)CurrentScene);

	private void UnloadScene(int buildIndex) => UnitySceneManager.UnloadSceneAsync(buildIndex);
}
