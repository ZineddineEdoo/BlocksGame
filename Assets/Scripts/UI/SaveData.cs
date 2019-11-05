using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AchievementManager;

[Serializable]
public class SaveData
{
	public float HighScore;
	public DateTime HighScoreDate;
	public List<AchievementID> Achievements;

	public SaveData()
	{
		Achievements = new List<AchievementID>();
	}

	public SaveData(SaveData saveData)
	{
		HighScore = saveData.HighScore;
		HighScoreDate = saveData.HighScoreDate;
		Achievements = saveData.Achievements;
	}

	public void SetAchievement(AchievementID id)
	{
		if (!Achievements.Any(a => a == id))
			Achievements.Add(id);
	}
}
