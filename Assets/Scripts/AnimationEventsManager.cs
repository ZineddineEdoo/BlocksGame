using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationEventsManager : MonoBehaviour
{
	public bool FadedIn { get; private set; }
	public bool FadedOut { get; private set; }

	void Awake()
	{
#if UNITY_EDITOR
		// Comment To Debug Scene Without Loading Global Scene
		if (SceneManager.Instance == null)
			SceneManager.LoadStart();
#endif
	}

	/// <summary>
	/// Used by Animation Event
	/// </summary>
	private void OnFadeInCompleted()
	{
		FadedIn = true;
		FadedOut = false;
	}

	/// <summary>
	/// Used by Animation Event
	/// </summary>
	private void OnFadeOutCompleted()
	{
		FadedIn = false;
		FadedOut = true;
	}

	public void FadeOut()
	{
		var animator = GetComponentInParent<Animator>();

		if (animator != null)
		{
			animator.SetBoolSafe("FadeIn", false);
			animator.SetBoolSafe("FadeOut", true);
		}
		else
			OnFadeOutCompleted();
	}
}
