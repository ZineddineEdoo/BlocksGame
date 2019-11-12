using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
	[SerializeField]
	private bool isFullDisplay = default;

	private TextMeshProUGUI textUI;
	private Color defaultColor;

	void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
		defaultColor = textUI.color;
	}

	void Update()
	{
		var formattedText = Globals.GetFormattedScoreText(Globals.Score, isFullDisplay);

#if !INSTANT
		textUI.SetText(formattedText);
#else
		if (Mathf.Abs(Globals.Score) >= Globals.INSTANT_SCORE_LIMIT)
		{
			textUI.color = Color.red;
			textUI.SetText($"[{formattedText}]");
		}
		else
		{
			textUI.color = defaultColor;
			textUI.SetText(formattedText);
		}
#endif
	}
}
