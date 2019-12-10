using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public event EventHandler<float> ScoreUpdated;
	public event EventHandler<float> BonusScoreUpdating;
	public event EventHandler<float> BonusScoreOneTimeUpdating;

	[SerializeField]
	[Tooltip("Per Second")]
	private float amount = default;

	[SerializeField]
	private bool isTimed = default;

	private GameManager gameManager;
	private float score;	

	public float Score
	{
		get => score;
		private set
		{
			if (score != value && !gameManager.IsRespawning)
			{
#if DEMO
				score = Mathf.Clamp(value, -Globals.INSTANT_SCORE_LIMIT, Globals.INSTANT_SCORE_LIMIT);
#else
				score = value;
#endif
				Globals.Score = score;

				ScoreUpdated?.Invoke(this, score);
			}
		}
	}	

	public float AmountPerSecond { get => amount; set => amount = value; }

	void Awake()
	{
		gameManager = GetComponent<GameManager>();
		gameManager.GameStarting += (s, e) =>
		{
			Score = 0f;
			Globals.Bonus = 0f;
		};
		gameManager.GameEnding += (s, e) =>
		{
			Globals.PreviousHighScore = SaveManager.CurrentSaveData.HighScore;

			if (score > SaveManager.CurrentSaveData.HighScore)
			{
				SaveManager.CurrentSaveData.SetHighScore(score);

				SaveManager.Save();
			}
		};
	}

	void Update()
	{
		if (isTimed && gameManager.IsGameStarted)
			IncreaseScore();
	}

	public void IncreaseScore() => Score += amount * Time.deltaTime;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="amt">Must be multiplied by Time.deltaTime</param>
	public void IncreaseScore(float amt) =>	Score += amt;

	/// <summary>
	/// Adds a One Time Bonus Score to Total Score
	/// </summary>
	/// <param name="bonus"></param>
	public void AddOneTimeBonus(float bonus)
	{
#if DEMO
		if (Mathf.Abs(score + bonus) > Globals.INSTANT_SCORE_LIMIT)
			bonus = Globals.INSTANT_SCORE_LIMIT - score;
#endif
		if (gameManager.IsGameStarted && bonus != 0f && !gameManager.IsRespawning)
		{
			BonusScoreOneTimeUpdating?.Invoke(this, bonus);

			if (bonus > 0)
				IncreaseScore(bonus);
			else
				DecreaseScore(bonus);

			Globals.Bonus += bonus;
		}
	}

	/// <summary>
	/// Adds a Bonus Score to Total Score
	/// </summary>
	/// <param name="position"></param>
	/// <param name="bonus">Must be multiplied by Time.deltaTime</param>
	public void AddBonus(float bonus)
	{
#if DEMO
		if (Mathf.Abs(score + bonus) > Globals.INSTANT_SCORE_LIMIT)
			bonus = Globals.INSTANT_SCORE_LIMIT - score;
#endif
		if (gameManager.IsGameStarted && bonus != 0f && !gameManager.IsRespawning)
		{
			BonusScoreUpdating?.Invoke(this, bonus);

			if (bonus > 0)
				IncreaseScore(bonus);
			else
				DecreaseScore(bonus);
		}
	}

	public void DecreaseScore() => Score -= amount * Time.deltaTime;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="amt">Must be multiplied by Time.deltaTime</param>
	public void DecreaseScore(float amt) => Score -= amt > 0 ? amt : -amt;
}
