using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class StringLoader
{
	[Serializable]
	public struct KeyValue
	{
		public string Key;
		public string Value;
	}

	[Serializable]
	public struct KeyValueArray
	{
		public KeyValue[] KeyValues;
	}

	[Serializable]
	public struct KeyHeaderDescription
	{
		public string Key;
		public string Header;
		public string Description;
	}

	[Serializable]
	public struct KeyHeaderDescriptionArray
	{
		public KeyHeaderDescription[] KeyValues;
	}

	public static KeyHeaderDescriptionArray LoadAchievementStrings()
	{
		TextAsset textAsset;

		if (Application.systemLanguage == SystemLanguage.French)
			textAsset = Resources.Load<TextAsset>("Achievements/French");
		else
			textAsset = Resources.Load<TextAsset>("Achievements/English");

		return JsonUtility.FromJson<KeyHeaderDescriptionArray>(textAsset.text);
	}
}
