using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DifficultyManager;
using static Globals;

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
		float min;
		float max;

		if (ScoreAbs >= MILLION)
		{		
			min = 100 * THOUSAND;
			max = MILLION;
		}
		else if (ScoreAbs >= 500 * THOUSAND)
		{		
			min = 50 * THOUSAND;
			max = 500 * THOUSAND;
		}
		else if (ScoreAbs >= 200 * THOUSAND)
		{		
			min = 20 * THOUSAND;
			max = 200 * THOUSAND;
		}
		else if (ScoreAbs >= 50 * THOUSAND)
		{
			min = 10 * THOUSAND;
			max = 50 * THOUSAND;
		}
		else if (ScoreAbs >= 10 * THOUSAND)
		{
			min = 1 * THOUSAND;
			max = 10 * THOUSAND;
		}
		else
		{
			min = 100;
			max = 10 * THOUSAND;
		}

		item.MaxBonus = Mathf.Sign(item.MaxBonus) * CalculateBonus(difficultyManager.CurrentDifficulty, min, max);
	}

	private float CalculateBonus(Difficulty difficulty, float min, float max)
	{
		float bonus = 0f;

		if (difficulty == Difficulty.Easy)
			bonus = UnityEngine.Random.Range(min, 0.1f * max);
		else if (difficulty == Difficulty.Medium)
			bonus = UnityEngine.Random.Range(0.1f * max, 0.4f * max);
		else if (difficulty == Difficulty.Hard)
			bonus = UnityEngine.Random.Range(0.4f * max, 0.7f * max);
		else if (difficulty == Difficulty.VeryHard)
			bonus = UnityEngine.Random.Range(0.7f * max, max);

		return bonus;
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
