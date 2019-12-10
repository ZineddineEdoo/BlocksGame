using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayAchievementNotification : MonoBehaviour
{
	[SerializeField]
	private AchievementManager achievementManager = default;

	[SerializeField]
	private AchievementUI achievementUI = default;

	private Queue<Achievement> achievements;
	private bool isShowing;

	void Awake()
	{
		achievements = new Queue<Achievement>();
		achievementManager.AchievementCompleted += (s, achievement) =>
		{
			achievements.Enqueue(achievement);
			ShowNotification();
		};
	}

	/// <summary>
	/// Called by Animation Event
	/// </summary>
	public void OnNotificationComplete()
	{
		isShowing = false;
		ShowNotification();
	}

	private void ShowNotification()
	{
		if (achievements.Count > 0 && !isShowing)
		{
			isShowing = true;

			achievementUI.Set(achievements.Dequeue());

			GetComponent<Animator>().SetTrigger("Notification");
		}
	}
}
