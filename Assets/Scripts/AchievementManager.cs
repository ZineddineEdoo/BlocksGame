using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AchievementManager
{
	public class Achievement
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
	}

	public static List<Achievement> Achievements { get; private set; }

	private static List<Achievement> LoadAchievements()
	{
		var achievements = new List<Achievement>();

		int id = 0;
		foreach (var kv in Strings.LoadAchievementStrings().KeyValues)
		{
			achievements.Add(new Achievement() { ID = id, Name = kv.Key, Detail = kv.Value });
			id++;
		}

		return achievements;
	}

	/// <summary>
	/// Loads Achievements Only if Achievements is Null <br/>
	/// Will Set Achievements to a Non-Null Value
	/// </summary>
	public static void Load()
	{
		if (Achievements == null)
			Achievements = LoadAchievements();
	}
}
