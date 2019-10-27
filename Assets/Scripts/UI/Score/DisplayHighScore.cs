using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayHighScore : MonoBehaviour
{
	private const float SPEED = 2000f;
	
	[SerializeField]
	private bool isFullDisplay = default;

	[SerializeField]
	[Tooltip("If True and Score is <= Previous High Score, Will Hide Parent GameObject")]
	private bool showOnlyIfNew = default;

	[SerializeField]
	[Header("If True, Requires SceneChanger Component in Parent Hierarchy")]
	private bool animate = default;

	private TextMeshProUGUI textUI;
	private bool isAnimating;
	private float currentScore;

	void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();

		// Hides High Score Object with Header
		if (showOnlyIfNew && Globals.Score <= Globals.PreviousHighScore)
			transform.parent.gameObject.SetActive(false);
		else
		{
			if (animate)
			{
				currentScore = Globals.PreviousHighScore;
				isAnimating = true;
			}
			else
				textUI.SetText(Globals.GetFormattedScoreText(SaveManager.CurrentSaveData.HighScore, isFullDisplay));
		}
	}

	void Update()
	{
		if (isAnimating && currentScore < SaveManager.CurrentSaveData.HighScore)
		{
			currentScore += SPEED * Time.deltaTime;
			textUI.SetText(Globals.GetFormattedScoreText(currentScore, isFullDisplay));
		}

		if (currentScore >= SaveManager.CurrentSaveData.HighScore)
		{
			isAnimating = false;
			textUI.SetText(Globals.GetFormattedScoreText(SaveManager.CurrentSaveData.HighScore, isFullDisplay));
		}
	}	
}
