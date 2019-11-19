using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private const int MAX_RETRIES = 2;

	// GameStarting -> GameStarted -> GameEnding
	public event EventHandler GameStarting;
	public event EventHandler GameStarted;
	public event EventHandler GameEnding;
	
	// True: GameStarted -> GameEnding	
	public bool IsGameStarted { get; private set; }
	public bool IsUILoaded { get; set; }
	// Not In Use
	//public float GameTime { get; private set; }

	private int numRetries;

	void Awake() =>	StartGame();

	public void StartGame()
	{
		if (!IsGameStarted)
		{
			StartCoroutine(StartGameDelayed());
		}
	}

	private IEnumerator StartGameDelayed()
	{
		GameStarting?.Invoke(this, null);

		yield return new WaitUntil(() => IsUILoaded);

		IsGameStarted = true;
		//GameTime = Time.time;
		Globals.CurrentStartTime = Time.time;

		GameStarted?.Invoke(this, null);
	}

	public void StopGame()
	{
		if (IsGameStarted)
		{
			if (numRetries < MAX_RETRIES)
			{
				Time.timeScale = 0f;

				OverlayManager.Instance.ShowOverlay($"Resume Game For {Globals.GetCompactFormattedScoreText(Globals.Score / 2f)}?", (result) =>
				{
					if (result == OverlayManager.Result.OK)
					{
						GetComponent<ScoreManager>().RespawnFor(Globals.Score / 2f);

						numRetries++;
						Time.timeScale = 1f;
						// Reset Player and Blocks?
					}
					else
						StopGameFully();
				});
			}
			else
				StopGameFully();
		}
	}

	private void StopGameFully()
	{
		numRetries = 0;

		IsGameStarted = false;
		//GameTime = 0f;
		Globals.CurrentStartTime = 0f;

		GameEnding?.Invoke(this, null);
	}
}
