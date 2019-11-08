using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Globals;

public class DifficultyManager : MonoBehaviour
{
	private const float DEFAULT_DELAY = 5f;

	public enum Difficulty
	{
		Easy,
		Medium,
		Hard,
		VeryHard
	}

	private enum ScoreDifficulty
	{
		Level0,
		Level1,
		Level2,
		Level3,
		Level4,
		Level5,
		Level6,
		Level7,
		Level8,
	}

	public event EventHandler<Difficulty> DifficultyChanging;

	private GameManager gameManager;
	private ScoreDifficulty scoreDifficulty;
	private Coroutine difficultyCoroutine;
	private float lastChangeTime;
	private Difficulty difficulty;

	public Difficulty CurrentDifficulty
	{
		get => difficulty;
		private set
		{
			DifficultyChanging?.Invoke(this, value);

			difficulty = value;
		}
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

		CurrentDifficulty = Difficulty.Easy;
	}

	void Update()
	{
		if (gameManager.IsGameStarted && Time.timeScale > 0f)
		{
			// gameManager.GameTime
			if (scoreDifficulty != ScoreDifficulty.Level8 && Mathf.Abs(Score) >= MILLION)
			{
				scoreDifficulty = ScoreDifficulty.Level8;
				
				if (difficultyCoroutine != null)
					StopCoroutine(difficultyCoroutine);

				CurrentDifficulty = Difficulty.VeryHard;
			}
			else if (scoreDifficulty != ScoreDifficulty.Level7 && Mathf.Abs(Score) >= 500 * THOUSAND)
			{
				scoreDifficulty = ScoreDifficulty.Level7;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 10 * MINUTE),
					(Difficulty.Hard, 5 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (scoreDifficulty != ScoreDifficulty.Level6 && Mathf.Abs(Score) >= 200 * THOUSAND)
			{
				scoreDifficulty = ScoreDifficulty.Level6;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 4 * MINUTE),
					(Difficulty.Hard, 3 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (scoreDifficulty != ScoreDifficulty.Level5 && Mathf.Abs(Score) >= 150 * THOUSAND)
			{
				scoreDifficulty = ScoreDifficulty.Level5;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 2 * MINUTE),
					(Difficulty.Hard, 1 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (scoreDifficulty != ScoreDifficulty.Level4 && Mathf.Abs(Score) >= 100 * THOUSAND)
			{
				scoreDifficulty = ScoreDifficulty.Level4;

				SetDifficultySequence(new[]
				{
					(Difficulty.VeryHard, 2 * MINUTE),
					(Difficulty.Hard, 0.5f * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (scoreDifficulty != ScoreDifficulty.Level3 && Mathf.Abs(Score) >= 10 * THOUSAND)
			{
				scoreDifficulty =  ScoreDifficulty.Level3;

				SetDifficultySequence(new[]
				{
					(Difficulty.Hard, 2 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (scoreDifficulty != ScoreDifficulty.Level2 && Mathf.Abs(Score) >= 5 * THOUSAND)
			{
				scoreDifficulty = ScoreDifficulty.Level2;

				SetDifficultySequence(new[]
				{
					(Difficulty.Hard, 1 * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (scoreDifficulty != ScoreDifficulty.Level1 && Mathf.Abs(Score) >= 1 * THOUSAND)
			{
				scoreDifficulty = ScoreDifficulty.Level1;

				SetDifficultySequence(new[]
				{
					(Difficulty.Hard, 0.5f * MINUTE),
					(Difficulty.Medium, -1f)
				});
			}
			else if (Time.time >= lastChangeTime + DEFAULT_DELAY)
			{
				scoreDifficulty = ScoreDifficulty.Level0;

				if (difficultyCoroutine != null)
					StopCoroutine(difficultyCoroutine);

				CurrentDifficulty = Difficulty.Easy;
				lastChangeTime = Time.time;
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
			CurrentDifficulty = difficulties[i].Difficulty;

			yield return new WaitForSeconds(difficulties[i].DelaySeconds);
		}

		CurrentDifficulty = difficulties.Last().Difficulty;

		difficultyCoroutine = null;
	}

	private void SetDifficultySequence((Difficulty Difficulty, float DelaySeconds)[] difficulties)
	{
		if (difficultyCoroutine != null)
			StopCoroutine(difficultyCoroutine);

		difficultyCoroutine = StartCoroutine(SetDifficulty(difficulties));
	}
}
