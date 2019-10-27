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

	void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		textUI.SetText(Globals.GetFormattedScoreText(Globals.Score, isFullDisplay));
	}
}
