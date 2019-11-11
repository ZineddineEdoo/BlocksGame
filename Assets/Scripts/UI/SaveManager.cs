using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
	private static readonly string SAVE_FILE_PATH = Path.Combine(Application.persistentDataPath, "SaveFile.bin");
	private static SaveData currentSaveData;

	public static SaveData CurrentSaveData
	{
		get
		{
			Load();
			return currentSaveData;
		}
		private set
		{
			currentSaveData = value;
			Save();
		}
	}

	public static void Save()
	{
#if !INSTANT
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
#endif
	}

	/// <summary>
	/// Loads Save Data Only if CurrentSaveData is Null <br/>
	/// Will Set CurrentSaveData to a Non-Null Value
	/// </summary>
	public static void Load()
	{
		if (currentSaveData == null)
		{
#if INSTANT
			currentSaveData = new SaveData();
#else
			try
			{
				using (var fileStream = new FileStream(SAVE_FILE_PATH, FileMode.Open, FileAccess.Read))
				{
					currentSaveData = (SaveData)new BinaryFormatter().Deserialize(fileStream);
					// If Version is not Latest Version
					// Convert Save Data based on currentSaveData.Version
					// currentSaveData.UpdateVersion();					
				}
			}
			catch (Exception)
			{
				currentSaveData = new SaveData();
			}
#endif
		}
	}
}
