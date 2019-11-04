using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
	[Serializable]
	public class AchievementSave
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
	public List<AchievementSave> Achievements;

	public SaveData()
	{
		Achievements = new List<AchievementSave>();
	}

	public SaveData(SaveData saveData)
	{
		HighScore = saveData.HighScore;
		HighScoreDate = saveData.HighScoreDate;
		Achievements = saveData.Achievements;
	}
}

public static class SaveManager
{
	private static string SAVE_FILE_PATH = Path.Combine(Application.persistentDataPath, "SaveFile.bin");
	private static SaveData currentSaveData;

	public static SaveData CurrentSaveData
	{
		get
		{
			Load();
			return currentSaveData;
		}
		set
		{
			currentSaveData = value;
			Save();
		}
	}

	public static void Save()
	{
		try
		{
			using (var fileStream = new FileStream(SAVE_FILE_PATH, FileMode.OpenOrCreate))
			{
				new BinaryFormatter().Serialize(fileStream, currentSaveData);
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Game couldn't be saved, Reason: {e.Message}");
		}
	}

	/// <summary>
	/// Loads Save Data Only if CurrentSaveData is Null <br/>
	/// Will Set CurrentSaveData to a Non-Null Value
	/// </summary>
	public static void Load()
	{
		if (currentSaveData == null)
		{
			try
			{
				using (var fileStream = new FileStream(SAVE_FILE_PATH, FileMode.Open, FileAccess.Read))
				{
					currentSaveData = (SaveData)new BinaryFormatter().Deserialize(fileStream);
				}
			}
			catch (Exception)
			{
				currentSaveData = new SaveData();
			}
		}
	}
}
