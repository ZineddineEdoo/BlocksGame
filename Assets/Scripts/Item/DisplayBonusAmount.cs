using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayBonusAmount : MonoBehaviour
{
	private float amount;

	void Update()
	{
		if (amount != GetComponentInParent<Item>().MaxBonus)
		{
			amount = GetComponentInParent<Item>().MaxBonus;

			GetComponent<TextMeshProUGUI>().SetText($"{Globals.GetCompactFormattedScoreText(amount)}");
		}
	}
}
