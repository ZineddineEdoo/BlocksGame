using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
	public class Achievement
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
	}

	public List<Achievement> Achievements { get; private set; }

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		Load();

		SceneManager.LoadSceneAsync(SceneChanger.MAIN_MENU_SCENE);
	}

	private List<Achievement> LoadAchievements()
	{
		var achievements = new List<Achievement>();

		int id = 0;
		foreach (var kv in StringLoader.LoadAchievementStrings().KeyValues)
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
	private void Load()
	{
		if (Achievements == null)
			Achievements = LoadAchievements();
	}

	public void Calculate()
	{
		foreach (var achievement in Achievements)
		{
			// TODO Check if Complete
			// Ex: SaveManager.CurrentSaveData.SetAchievement(0, true);
			// Raise Event for Popup
		}
	}
}
