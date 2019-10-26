using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
	private TextMeshProUGUI textUI;

	void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		textUI.SetText($"{Globals.Score:0}");
	}
}
