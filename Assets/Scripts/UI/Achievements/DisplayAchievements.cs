using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		var manager = AchievementManager.Instance;
		AchievementLoader loader;

		if (manager == null)
			loader = new AchievementLoader();
		else
			loader = manager.Loader;

		headerText.SetText($"Achievements ({SaveManager.CurrentSaveData.Achievements.Count}/{loader.Achievements.Count})");

		foreach (var achievement in loader.Achievements)
		{
			var achievementUI = Instantiate(achievementPrefab, achievementsGroup.transform);

			achievementUI.Set(achievement);
		}
	}
}
