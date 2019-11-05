using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
	[Serializable]
	public class AchievementData
	{
		[SerializeField]
		private int id;
		[SerializeField]
		private bool isComplete;

		public int ID { get => id; set => id = value; }
		public bool IsComplete { get => isComplete; set => isComplete = value; }
	}

	public float HighScore;
	public DateTime HighScoreDate;
	public List<AchievementData> Achievements;

	public SaveData()
	{
		Achievements = new List<AchievementData>();
	}

	public SaveData(SaveData saveData)
	{
		HighScore = saveData.HighScore;
		HighScoreDate = saveData.HighScoreDate;
		Achievements = saveData.Achievements;
	}

	public void SetAchievement(int id, bool isComplete)
	{
		var found = Achievements.FirstOrDefault(a => a.ID == id);

		if (found != default)
			found.IsComplete = isComplete;
		else
		{
			found = new AchievementData()
			{
				IsComplete = isComplete
			};

			Achievements.Add(found);
		}
	}
}
