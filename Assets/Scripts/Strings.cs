using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Strings
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

	public static KeyValueArray LoadAchievementStrings()
	{
		TextAsset textAsset;

		if (Application.systemLanguage == SystemLanguage.French)
			textAsset = Resources.Load<TextAsset>("Achievements/French");
		else
			textAsset = Resources.Load<TextAsset>("Achievements/English");

		return JsonUtility.FromJson<KeyValueArray>(textAsset.text);
	}
}
