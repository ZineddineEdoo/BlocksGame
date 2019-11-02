using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public struct SaveData
{
	public readonly float HighScore;
	public readonly DateTime HighScoreDate;

	public SaveData(float highScore, DateTime highScoreDate)
	{
		HighScore = highScore;
		HighScoreDate = highScoreDate;
	}
}

public static class SaveManager
{
	private static string SAVE_FILE_PATH = Path.Combine(Application.persistentDataPath, "SaveFile.bin");
	private static SaveData? currentSaveData;

	public static SaveData CurrentSaveData
	{
		get
		{
			if (currentSaveData == null)
				Load();

			return (SaveData) currentSaveData;
		}
		set
		{
			Save(value);			
		}
	}

	private static void Save(SaveData saveData)
	{
		try
		{
			using (var fileStream = new FileStream(SAVE_FILE_PATH, FileMode.OpenOrCreate))
			{
				new BinaryFormatter().Serialize(fileStream, saveData);
				currentSaveData = saveData;
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Game couldn't be saved, Reason: {e.Message}");
		}
	}

	/// <summary>
	/// Will Set CurrentSaveData to a Non-Null Value
	/// </summary>
	private static void Load()
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
