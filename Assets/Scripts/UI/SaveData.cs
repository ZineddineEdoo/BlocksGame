using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AchievementLoader;
using static AchievementLoader.Achievement;

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

	/// <summary>
	/// Sets High Score to 'highScore' and High Score Date to 'Now'
	/// </summary>
	/// <param name="highScore"></param>
	public void SetHighScore(float highScore)
	{
		HighScore = highScore;
		HighScoreDate = DateTime.Now;
	}

	public void SetAchievement(AchievementID id)
	{
		if (!Achievements.Any(a => a == id))
			Achievements.Add(id);
	}
}
