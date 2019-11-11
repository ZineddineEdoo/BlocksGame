using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AchievementLoader;
using static AchievementLoader.Achievement;

[Serializable]
public class SaveData
{
	public enum VersionCode
	{
		Version1
	}

	public VersionCode Version;
	public float HighScore;
	public DateTime HighScoreDate;
	public List<AchievementID> Achievements;

	public SaveData()
	{
		Version = GetLatestVersion();
		Achievements = new List<AchievementID>();
	}

	public SaveData(SaveData saveData)
	{
		Version = saveData.Version;
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

	public void UpdateVersion() => Version = GetLatestVersion();

	private VersionCode GetLatestVersion()
	{
		var versions = Enum.GetValues(typeof(VersionCode));

		return (VersionCode)versions.GetValue(versions.Length - 1);
	}
}
