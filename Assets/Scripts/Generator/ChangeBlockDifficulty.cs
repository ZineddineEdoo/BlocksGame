using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DifficultyManager;

public class ChangeBlockDifficulty : MonoBehaviour
{
	[SerializeField]
	private DifficultyManager difficultyManager = default;
	
	private Generator generator;

	void Awake()
	{
		generator = GetComponent<Generator>();

		if (generator == null)
			Debug.LogError($"{nameof(Generator)} Component must be on this GameObject");
		else
			difficultyManager.DifficultyChanged += DifficultyManager_DifficultyChanged;
	}

	private void DifficultyManager_DifficultyChanged(object sender, Difficulty difficulty)
	{
		if (difficulty == Difficulty.Easy)
			generator.SetDifficulty(UnityEngine.Random.Range(0, 2));
		else if (difficulty == Difficulty.Medium)
			generator.SetDifficulty(UnityEngine.Random.Range(2, 4));
		else if (difficulty == Difficulty.Hard)
			generator.SetDifficulty(4);
		else if (difficulty == Difficulty.VeryHard)
			generator.SetDifficulty(5);
	}
}
