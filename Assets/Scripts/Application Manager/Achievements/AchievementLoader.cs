using System;
using System.Collections.Generic;
using System.Linq;
using static AchievementLoader.Achievement;

public class AchievementLoader
{
	public class Achievement
	{
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
			ScoreN1M
		}

		public AchievementID ID { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
	}

	public List<Achievement> Achievements { get; private set; }

	public AchievementLoader() => Load();

	private List<Achievement> LoadAchievements()
	{
		var achievements = new List<Achievement>();

		foreach (var kv in StringLoader.LoadAchievementStrings().KeyValues)
		{
			if (Enum.TryParse(kv.Key, out AchievementID id) && !achievements.Any(a => a.ID == id))
			{
				achievements.Add(new Achievement()
				{
					ID = id,
					Name = kv.Header,
					Detail = kv.Description
				});
			}
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
