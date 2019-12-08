using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private const int MAX_RETRIES = 2;
	private const float RESPAWN_DELAY = 1f;		// Seconds

	// GameStarting -> GameStarted -> GameEnding
	public event EventHandler GameStarting;
	public event EventHandler GameStarted;
	public event EventHandler GameEnding;
	
	// True: GameStarted -> GameEnding	
	public bool IsGameStarted { get; private set; }
	public bool IsUILoaded { get; set; }
	// Not In Use
	//public float GameTime { get; private set; }

	public bool IsRespawning
	{
		get
		{
			bool respawning = true;

			if (lastRespawnTime == 0f || (lastRespawnTime > 0f && Time.time >= lastRespawnTime + RESPAWN_DELAY))
			{
				respawning = false;
				lastRespawnTime = 0f;
			}

			return respawning;
		}
	}

	private int retriesLeft;
	private float lastRespawnTime;

	void Awake() =>	StartGame();

	public void StartGame()
	{
		if (!IsGameStarted)
		{
			this.RestartCoroutine(StartGameDelayed(), nameof(StartGameDelayed));
		}
	}

	private IEnumerator StartGameDelayed()
	{
		retriesLeft = MAX_RETRIES;
		GameStarting?.Invoke(this, null);
		
		yield return new WaitUntil(() => IsUILoaded);

		IsGameStarted = true;
		//GameTime = Time.time;
		Globals.CurrentStartTime = Time.time;
		
		Time.timeScale = 1f;

		GameStarted?.Invoke(this, null);

		this.RemoveCoroutine(nameof(StartGameDelayed));
	}

	public void StopGame()
	{
		if (IsGameStarted && !IsRespawning)
		{
			if (retriesLeft > 0)
			{
				Time.timeScale = 0f;

				var msg = $"Resume Game at Score {Globals.GetCompactFormattedScoreText(Globals.Score / 2f)} ?\r\n\r\n" +
					$"{retriesLeft - 1} {(retriesLeft == 2 ? "Retry" : "Retries")} Left";

				OverlayManager.Instance.ShowOverlay(msg, OverlayManager.ActionOptions.YesNo, (result) =>
				{
					if (result == OverlayManager.Result.OK)
					{
						GetComponent<ScoreManager>().DecreaseScore(Globals.Score / 2f);

						retriesLeft--;
						lastRespawnTime = Time.time;
						Time.timeScale = 1f;
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
		IsGameStarted = false;
		//GameTime = 0f;
		Globals.CurrentStartTime = 0f;

		GameEnding?.Invoke(this, null);
	}
}
