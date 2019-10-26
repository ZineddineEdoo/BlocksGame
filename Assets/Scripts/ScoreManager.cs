using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public event EventHandler<float> ScoreUpdated;
	public event EventHandler<(float Bonus, Vector2 Position)> BonusScoreUpdating;

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
		protected set
		{
			score = value;
			
			Globals.Score = score;
			ScoreUpdated?.Invoke(this, score);
		}
	}	

	public float AmountPerSecond { get => amount; set => amount = value; }

	void Awake()
	{
		gameManager = GetComponent<GameManager>();
		gameManager.GameStarting += (s, e) =>
		{
			Score = 0f;
		};
		gameManager.GameEnding += (s, e) =>
		{
			Globals.PreviousHighScore = SaveManager.CurrentSaveData.HighScore;

			if (score > SaveManager.CurrentSaveData.HighScore)
				SaveManager.CurrentSaveData = new SaveData(score);
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
	/// Adds a bonus score to Total Score
	/// </summary>
	/// <param name="position"></param>
	/// <param name="bonus">Must be multiplied by Time.deltaTime</param>
	public void AddBonus(Vector2 position, float bonus)
	{
		if (gameManager.IsGameStarted && bonus != 0f)
		{
			BonusScoreUpdating?.Invoke(this, (bonus, position));
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
