using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
	/// <summary>
	/// Order Must match Json File
	/// </summary>
	public enum AchievementID
	{
		Score1K,
		Score10K
	}

	public class Achievement
	{
		public AchievementID ID { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
	}

	public event EventHandler<Achievement> AchievementCompleted;

	public List<Achievement> Achievements { get; private set; }

	void Awake()
	{
		Load();

		SceneManager.LoadSceneAsync(SceneChanger.MAIN_MENU_SCENE);
	}

	private List<Achievement> LoadAchievements()
	{
		var achievements = new List<Achievement>();

		int id = 0;
		foreach (var kv in StringLoader.LoadAchievementStrings().KeyValues)
		{
			achievements.Add(new Achievement() 
			{ 
				ID = (AchievementID) Enum.Parse(typeof(AchievementID), id.ToString()), 
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

	void Update()
	{
		Calculate();
	}

	private void Calculate()
	{
		// Every Achievement Not In CurrentSaveData
		foreach (var achievement in Achievements.Where(a => !SaveManager.CurrentSaveData.Achievements.Any(i => i == a.ID)))
		{
			if (achievement.ID == AchievementID.Score1K && Globals.Score >= Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score1K);
			else if (achievement.ID == AchievementID.Score10K && Globals.Score >= 10 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score10K);
		}
	}

	private void ShowAchievementComplete(AchievementID id)
	{
		SaveManager.CurrentSaveData.SetAchievement(id);

		AchievementCompleted?.Invoke(this, Achievements.First(a => a.ID == id));

		SaveManager.Save();
	}
}
