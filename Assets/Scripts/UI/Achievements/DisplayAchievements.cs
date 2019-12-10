using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAchievements : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI headerText = default;

	[SerializeField]
	private VerticalLayoutGroup achievementsGroup = default;

	[SerializeField]
	private AchievementUI achievementPrefab = default;

	void Awake()
	{
#if UNITY_EDITOR
		if (AchievementManager.Instance == null)
		{
			var tempManagerObj = Instantiate(new GameObject());
			tempManagerObj.AddComponent<StringManager>();
			tempManagerObj.AddComponent<AchievementManager>();
		}
#endif
		var achievements = AchievementManager.Instance.Achievements;

		headerText.SetText($"Achievements ({SaveManager.CurrentSaveData.Achievements.Count}/{achievements.Count})");

		foreach (var achievement in achievements)
		{
			var achievementUI = Instantiate(achievementPrefab, achievementsGroup.transform);

			achievementUI.Set(achievement);
		}
	}
}
