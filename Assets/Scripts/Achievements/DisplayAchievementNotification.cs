using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static AchievementManager;

public class DisplayAchievementNotification : MonoBehaviour
{
	[SerializeField]
	private AchievementManager achievementManager = default;

	[SerializeField]
	private TextMeshProUGUI headerText = default;

	[SerializeField]
	private TextMeshProUGUI detailsText = default;

	private Queue<Achievement> achievements;
	private bool isShowing;

	void Awake()
	{
		achievements = new Queue<Achievement>();
		achievementManager.AchievementCompleted += AchievementManager_AchievementCompleted;
	}

	private void AchievementManager_AchievementCompleted(object sender, Achievement achievement)
	{
		achievements.Enqueue(achievement);
		ShowNotification();
	}

	/// <summary>
	/// Called by Animation Event
	/// </summary>
	private void OnNotificationComplete()
	{
		isShowing = false;
		ShowNotification();
	}

	private void ShowNotification()
	{
		if (achievements.Count > 0 && !isShowing)
		{
			isShowing = true;

			var achievement = achievements.Dequeue();
			headerText.SetText(achievement.Name);
			detailsText.SetText(achievement.Detail);

			GetComponent<Animator>().SetTrigger("Notification");
		}
	}
}
