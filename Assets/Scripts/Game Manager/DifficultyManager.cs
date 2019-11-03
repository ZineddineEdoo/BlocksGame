using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	public enum Difficulty
	{
		Easy,
		Medium,
		Hard,
		VeryHard
	}

	public event EventHandler<Difficulty> DifficultyChanging;

	[SerializeField]
	private float minTime = default;

	[SerializeField]
	private float maxTime = default;

	private GameManager gameManager;
	private float lastChangeTime;
	private float randomDelay;

	void OnValidate()
	{
		if (minTime < 0f)
			minTime = 0f;

		if (maxTime < minTime)
			maxTime = minTime + 1f;
	}

	void Awake()
	{
		gameManager = GetComponent<GameManager>();
		
		if (gameManager == null)
			Debug.LogError($"{nameof(GameManager)} Component must be on this GameObject");

		gameManager.GameStarting += (s, e) => Initialise();
	}

	private void Initialise()
	{
		lastChangeTime = 0f;
		randomDelay = UnityEngine.Random.Range(minTime, maxTime);
	}

	void Update()
	{
		if (gameManager.IsGameStarted && Time.time >= lastChangeTime + randomDelay)
		{
			// Calculate Difficulty
			// Globals.Score
			// gameManager.GameTime



			DifficultyChanging?.Invoke(this, Difficulty.VeryHard);

			randomDelay = UnityEngine.Random.Range(minTime, maxTime);
		}
	}
}
