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
	private FontStyles defaultStyle;

	void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
		defaultColor = textUI.color;
		defaultStyle = textUI.fontStyle;
	}

	void Update()
	{
		textUI.SetText(Globals.GetFormattedScoreText(Globals.Score, isFullDisplay));

#if INSTANT
		if (Mathf.Abs(Globals.Score) >= Globals.INSTANT_SCORE_LIMIT)
		{
			textUI.color = Color.red;
			textUI.fontStyle = FontStyles.Italic;
		}
		else
		{
			textUI.color = defaultColor;
			textUI.fontStyle = defaultStyle;
		}
#endif
	}
}
