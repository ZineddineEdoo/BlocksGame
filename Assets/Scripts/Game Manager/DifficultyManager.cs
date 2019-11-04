using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	private const float MINUTE = 60f;

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
	private float currentScoreDifficulty;
	private Coroutine difficultyCoroutine;

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

		Initialise();
		gameManager.GameStarting += (s, e) => Initialise();
	}

	private void Initialise()
	{
		if (difficultyCoroutine != null)
		{
			StopCoroutine(difficultyCoroutine);
			difficultyCoroutine = null;
		}

		DifficultyChanging?.Invoke(this, Difficulty.Medium);
	}

	void Update()
	{
		if (gameManager.IsGameStarted)
		{
			// Globals.Score
			// gameManager.GameTime
			if (currentScoreDifficulty != Globals.MILLION && Globals.Score >= Globals.MILLION)
			{
				currentScoreDifficulty = Globals.MILLION;
				
				if (difficultyCoroutine != null)
					StopCoroutine(difficultyCoroutine);

				DifficultyChanging?.Invoke(this, Difficulty.VeryHard);
			}
			else if (currentScoreDifficulty != 500 * Globals.THOUSAND && Globals.Score >= 500 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 500 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 10 * MINUTE),
					(Difficulty.Hard, 5 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (currentScoreDifficulty != 200 * Globals.THOUSAND && Globals.Score >= 200 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 200 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 4 * MINUTE),
					(Difficulty.Hard, 3 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (currentScoreDifficulty != 150 * Globals.THOUSAND && Globals.Score >= 150 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 150 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 2 * MINUTE),
					(Difficulty.Hard, 1 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (currentScoreDifficulty != 100 * Globals.THOUSAND && Globals.Score >= 100 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 100 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 2 * MINUTE),
					(Difficulty.Hard, 0.5f * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (currentScoreDifficulty != 10 * Globals.THOUSAND && Globals.Score >= 10 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 10 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.Hard, 2 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (currentScoreDifficulty != 5 * Globals.THOUSAND && Globals.Score >= 5 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 5 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.Hard, 1 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (currentScoreDifficulty != 1 * Globals.THOUSAND && Globals.Score >= 1 * Globals.THOUSAND)
			{
				currentScoreDifficulty = 1 * Globals.THOUSAND;

				SetDifficultySequence(new[]
				{
					(Difficulty.Hard, 0.5f * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
		}
	}

	/// <summary>
	/// Sets Difficulty Sequence <br/>
	/// Last Element's Delay Parameter isn't used
	/// </summary>
	/// <param name="difficulties">Difficulty Sequence</param>
	/// <returns></returns>
	private IEnumerator SetDifficulty((Difficulty Difficulty, float DelaySeconds)[] difficulties)
	{
		for (int i = 0; i < difficulties.Length - 1; i++)
		{
			DifficultyChanging?.Invoke(this, difficulties[i].Difficulty);

			yield return new WaitForSeconds(difficulties[i].DelaySeconds);
		}

		DifficultyChanging?.Invoke(this, difficulties.Last().Difficulty);

		difficultyCoroutine = null;
	}

	private void SetDifficultySequence((Difficulty Difficulty, float DelaySeconds)[] difficulties)
	{
		if (difficultyCoroutine != null)
			StopCoroutine(difficultyCoroutine);

		difficultyCoroutine = StartCoroutine(SetDifficulty(difficulties));
	}
}
