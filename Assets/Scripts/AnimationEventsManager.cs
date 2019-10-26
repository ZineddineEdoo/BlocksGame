using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationEventsManager : MonoBehaviour
{
	public bool FadedIn { get; private set; }
	public bool FadedOut { get; private set; }

	private void OnFadeInCompleted()
	{
		FadedIn = true;
		FadedOut = false;
	}

	private void OnFadeOutCompleted()
	{
		FadedIn = false;
		FadedOut = true;
	}
}
