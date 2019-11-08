using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DifficultyManager;

public class ChangeBonusDifficulty : MonoBehaviour
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
		{
			generator.ItemGenerated += Generator_ItemGenerated;
			difficultyManager.DifficultyChanging += DifficultyManager_DifficultyChanging;
		}
	}

	private void Generator_ItemGenerated(object sender, Item item)
	{
		if (difficultyManager.CurrentDifficulty == Difficulty.Easy)
			item.MaxBonus = Mathf.Sign(item.MaxBonus) * 1 * Globals.THOUSAND;
		else if (difficultyManager.CurrentDifficulty == Difficulty.Medium)
			item.MaxBonus = Mathf.Sign(item.MaxBonus) * 5 * Globals.THOUSAND;
		else if (difficultyManager.CurrentDifficulty == Difficulty.Hard)
			item.MaxBonus = Mathf.Sign(item.MaxBonus) * 10 * Globals.THOUSAND;
		else if (difficultyManager.CurrentDifficulty == Difficulty.VeryHard)
			item.MaxBonus = Mathf.Sign(item.MaxBonus) * 100 * Globals.THOUSAND;
	}

	private void DifficultyManager_DifficultyChanging(object sender, Difficulty difficulty)
	{
		if (difficulty == Difficulty.Easy)
			generator.SetDifficulty(UnityEngine.Random.Range(0, 2));
		else if (difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
			generator.SetDifficulty(1);
		else if (difficulty == Difficulty.VeryHard)
			generator.SetDifficulty(2);
	}
}
