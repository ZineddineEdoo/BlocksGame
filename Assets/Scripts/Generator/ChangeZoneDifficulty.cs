using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DifficultyManager;

public class ChangeZoneDifficulty : MonoBehaviour
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
			difficultyManager.DifficultyChanging += DifficultyManager_DifficultyChanging;
	}

	private void DifficultyManager_DifficultyChanging(object sender, Difficulty difficulty)
	{
		if (difficulty == Difficulty.Easy)
			generator.SetDifficulty(0);
		else if (difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
			generator.SetDifficulty(1);
		else if (difficulty == Difficulty.VeryHard)
			generator.SetDifficulty(2);
	}
}
