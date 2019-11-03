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
			difficultyManager.DifficultyChanged += DifficultyManager_DifficultyChanged;
	}

	private void DifficultyManager_DifficultyChanged(object sender, Difficulty difficulty)
	{
		if (difficulty == Difficulty.Easy)
			generator.SetDifficulty(0);
		else if (difficulty == Difficulty.Medium)
			generator.SetDifficulty(1);
		else if (difficulty == Difficulty.Hard || difficulty == Difficulty.VeryHard)
			generator.SetDifficulty(2);
	}
}
