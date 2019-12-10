using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI headerText = default;

	[SerializeField]
	private TextMeshProUGUI detailsText = default;

	[SerializeField]
	private Color incompleteColor = default;

	[SerializeField]
	private Color completeColor = default;

	public void Set(Achievement achievement)
	{
		headerText.SetText(achievement.Name);
		detailsText.SetText(achievement.Detail);

		if (SaveManager.CurrentSaveData.Achievements.Contains(achievement.ID))
			GetComponentInParent<RawImage>().color = completeColor;
		else
			GetComponentInParent<RawImage>().color = incompleteColor;
	}
}
