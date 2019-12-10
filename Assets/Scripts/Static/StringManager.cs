using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StringManager : MonoBehaviour
{
	public event EventHandler Loaded;

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

	public static StringManager Instance { get; private set; }

	public Dictionary<string, string> Strings { get; private set; }

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
		{
			Instance = this;

			Strings = new Dictionary<string, string>();
			LoadStrings();
		}
	}

	public void LoadStrings()
	{
		TextAsset stringsAsset;

		if (Application.systemLanguage == SystemLanguage.French)
			stringsAsset = Resources.Load<TextAsset>("Strings/French");
		else
			stringsAsset = Resources.Load<TextAsset>("Strings/English");

		foreach (var kv in JsonUtility.FromJson<KeyValueArray>(stringsAsset.text).KeyValues)
		{
			if (!Strings.ContainsKey(kv.Key))
				Strings.Add(kv.Key, kv.Value);
		}

		Loaded?.Invoke(this, null);
	}
}
