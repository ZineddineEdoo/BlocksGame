using System;
using System.Collections.Generic;
using static AchievementLoader.Achievement;

public class AchievementLoader
{	
	public class Achievement
	{
		/// <summary>
		/// Order Must match Json File
		/// </summary>
		public enum AchievementID
		{
			Score1K,
			Score10K,
			Score100K,
			Score1M,
			Score10M,
			Score100M,
			ScoreN1K,
			ScoreN10K,
			ScoreN100K,
			ScoreN1M,
		}

		public AchievementID ID { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
	}

	public List<Achievement> Achievements { get; private set; }

	public AchievementLoader()
	{
		Load();
	}

	private List<Achievement> LoadAchievements()
	{
		var achievements = new List<Achievement>();

		int id = 0;
		foreach (var kv in StringLoader.LoadAchievementStrings().KeyValues)
		{
			achievements.Add(new Achievement()
			{
				ID = (AchievementID)Enum.Parse(typeof(AchievementID), id.ToString()),
				Name = kv.Key,
				Detail = kv.Value
			});
			id++;
		}

		return achievements;
	}

	/// <summary>
	/// Loads Achievements Only if Achievements is Null <br/>
	/// Will Set Achievements to a Non-Null Value
	/// </summary>
	private void Load()
	{
		if (Achievements == null)
			Achievements = LoadAchievements();
	}
}
