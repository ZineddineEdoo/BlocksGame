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
			if (scoreDifficulty != ScoreDifficulty.Level8 && ScoreAbs >= MILLION)
				SetLevel8();
			else if (scoreDifficulty != ScoreDifficulty.Level7 && ScoreAbs >= 500 * THOUSAND)
				SetLevel7();
			else if (scoreDifficulty != ScoreDifficulty.Level6 && ScoreAbs >= 200 * THOUSAND)
				SetLevel6();
			else if (scoreDifficulty != ScoreDifficulty.Level5 && ScoreAbs >= 150 * THOUSAND)
				SetLevel5();
			else if (scoreDifficulty != ScoreDifficulty.Level4 && ScoreAbs >= 100 * THOUSAND)
				SetLevel4();
			else if (scoreDifficulty != ScoreDifficulty.Level3 && ScoreAbs >= 10 * THOUSAND)
				SetLevel3();
			else if (scoreDifficulty != ScoreDifficulty.Level2 && ScoreAbs >= 5 * THOUSAND)
				SetLevel2();
			else if (scoreDifficulty != ScoreDifficulty.Level1 && ScoreAbs >= 1 * THOUSAND)
				SetLevel1();
			else if (Time.time >= lastChangeTime + DEFAULT_DELAY)
				SetLevel0();
		}
	}

	private void SetLevel8()
	{
		scoreDifficulty = ScoreDifficulty.Level8;

		if (difficultyCoroutine != null)
			StopCoroutine(difficultyCoroutine);

		CurrentDifficulty = Difficulty.VeryHard;
	}

	private void SetLevel7()
	{
		scoreDifficulty = ScoreDifficulty.Level7;

		SetDifficultySequence(new[]
		{
			(Difficulty.VeryHard, 10 * MINUTE),
			(Difficulty.Hard, 5 * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}

	private void SetLevel6()
	{
		scoreDifficulty = ScoreDifficulty.Level6;

		SetDifficultySequence(new[]
		{
			(Difficulty.VeryHard, 4 * MINUTE),
			(Difficulty.Hard, 3 * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}

	private void SetLevel5()
	{
		scoreDifficulty = ScoreDifficulty.Level5;

		SetDifficultySequence(new[]
		{
			(Difficulty.VeryHard, 2 * MINUTE),
			(Difficulty.Hard, 1 * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}
	
	private void SetLevel4()
	{
		scoreDifficulty = ScoreDifficulty.Level4;

		SetDifficultySequence(new[]
		{
			(Difficulty.VeryHard, 2 * MINUTE),
			(Difficulty.Hard, 0.5f * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}
	
	private void SetLevel3()
	{
		scoreDifficulty = ScoreDifficulty.Level3;

		SetDifficultySequence(new[]
		{
			(Difficulty.Hard, 2 * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}

	private void SetLevel2()
	{
		scoreDifficulty = ScoreDifficulty.Level2;

		SetDifficultySequence(new[]
		{
			(Difficulty.Hard, 1 * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}

	private void SetLevel1()
	{
		scoreDifficulty = ScoreDifficulty.Level1;

		SetDifficultySequence(new[]
		{
			(Difficulty.Hard, 0.5f * MINUTE),
			(Difficulty.Medium, -1f)
		});
	}

	private void SetLevel0()
	{
		scoreDifficulty = ScoreDifficulty.Level0;

		if (difficultyCoroutine != null)
			StopCoroutine(difficultyCoroutine);

		CurrentDifficulty = Difficulty.Easy;
		lastChangeTime = Time.time;
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
